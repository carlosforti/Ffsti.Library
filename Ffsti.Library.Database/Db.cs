using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Dapper;

namespace Ffsti.Library.Database
{
	public class Db : IDisposable
	{
		//SqlServer Connection String - .NET Framework Data Provider for SQL Server
		//Provider - System.Data.SqlClient
		//
		//Standard Security
		//Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
		//
		//Trusted Connection
		//Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;

		//Oracle Connection String - Oracle Data Provider for .NET / ODP.NET
		//Provider -  Oracle.DataAccess.Client
		//
		//Using TNS
		//Data Source=TORCL;User Id=myUsername;Password=myPassword;
		//
		//Using Integrated Security
		//Data Source=TORCL;Integrated Security=SSPI;
		//
		//Using ODP.NET without tnsnames.ora
		//Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;

		private IDbConnection connection = null;
		private IDbTransaction transaction = null;

		private string connectionString;
		private string providerName;

		private DbProviderFactory dbProviderFactory = null;

		protected IDbConnection Connection
		{
			get { return connection; }
			set { connection = value; }
		}

		public ConnectionState State
		{
			get { return this.connection.State; }
		}

		public IDbTransaction Transaction
		{
			get { return this.transaction; }
		}

		public Db(string connectionString, string providerName)
		{
			this.connectionString = connectionString;
			this.providerName = providerName;

			try
			{
				dbProviderFactory = DbProviderFactories.GetFactory(providerName);
			}
			catch
			{
				throw;
			}

			connection = dbProviderFactory.CreateConnection();
			connection.ConnectionString = this.connectionString;
			connection.Open();
		}

		public void OpenConnection()
		{
			connection = dbProviderFactory.CreateConnection();
			connection.ConnectionString = this.connectionString;
			connection.Open();
		}

		public void CloseConnection()
		{
			connection.Close();
		}

		public IDbCommand GetCommand(string commandText)
		{
			var comm = connection.CreateCommand();
			comm.CommandText = commandText;
			if (transaction != null)
				comm.Transaction = transaction;

			return comm;
		}

		public DataTable GetDataTable(string query)
		{
			using (var dataAdapter = dbProviderFactory.CreateDataAdapter())
			{
				dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
				DataTable table = new DataTable();
				dataAdapter.Fill(table);

				return table;
			}
		}

		public DataTable GetDataTable(string query, params IDataParameter[] parameters)
		{
			using (var dataAdapter = dbProviderFactory.CreateDataAdapter())
			{
				dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
				for (int i = 0; i < parameters.Length; i++)
				{
					dataAdapter.SelectCommand.Parameters.Add(parameters[i]);
				};

				DataTable table = new DataTable();
				dataAdapter.Fill(table);

				return table;
			}
		}

		public DataTable GetDataTable(IDbCommand command)
		{
			using (var dataAdapter = dbProviderFactory.CreateDataAdapter())
			{
				dataAdapter.SelectCommand = (DbCommand)command;

				DataTable table = new DataTable();
				dataAdapter.Fill(table);

				return table;
			}
		}

		public IDataReader ExecuteReader(string commandText)
		{
			return GetCommand(commandText).ExecuteReader();
		}

		public object ExecuteScalar(string commandText)
		{
			return GetCommand(commandText).ExecuteScalar();
		}

		public int ExecuteNonQuery(string commandText)
		{
			return GetCommand(commandText).ExecuteNonQuery();
		}

		public IEnumerable<T> Query<T>(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.Query<T>(commandText, param, transaction);
		}

		public int Execute(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.Execute(commandText, param, transaction);
		}

		public IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.ExecuteReader(commandText, param, transaction);
		}

		public T ExecuteScalar<T>(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.ExecuteScalar<T>(commandText, param, transaction);
		}

		public bool OpenTransaction()
		{
			transaction = connection.BeginTransaction();
			return (transaction != null);
		}

		public void CommitTransaction()
		{
			transaction.Commit();
			transaction.Dispose();
			transaction = null;
		}

		public void RollbackTransaction()
		{
			transaction.Rollback();
			transaction.Dispose();
			transaction = null;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (transaction != null) transaction.Dispose();
				if (dbProviderFactory != null) dbProviderFactory = null;

				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
