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

		public virtual void OpenConnection()
		{
			connection = dbProviderFactory.CreateConnection();
			connection.ConnectionString = this.connectionString;
			connection.Open();
		}

		public virtual void CloseConnection()
		{
			connection.Close();
		}

		public virtual IDbCommand GetCommand(string commandText)
		{
			var comm = connection.CreateCommand();
			comm.CommandText = commandText;
			if (transaction != null)
				comm.Transaction = transaction;

			return comm;
		}

		public virtual DataTable GetDataTable(string query)
		{
			using (var dataAdapter = dbProviderFactory.CreateDataAdapter())
			{
				dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
				DataTable table = new DataTable();
				dataAdapter.Fill(table);

				return table;
			}
		}

		public virtual DataTable GetDataTable(string query, params IDataParameter[] parameters)
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

		public virtual DataTable GetDataTable(IDbCommand command)
		{
			using (var dataAdapter = dbProviderFactory.CreateDataAdapter())
			{
				dataAdapter.SelectCommand = (DbCommand)command;

				DataTable table = new DataTable();
				dataAdapter.Fill(table);

				return table;
			}
		}

		public virtual IDataReader ExecuteReader(string commandText)
		{
			return GetCommand(commandText).ExecuteReader();
		}

		public virtual object ExecuteScalar(string commandText)
		{
			return GetCommand(commandText).ExecuteScalar();
		}

		public virtual int ExecuteNonQuery(string commandText)
		{
			return GetCommand(commandText).ExecuteNonQuery();
		}

		public virtual IEnumerable<T> Query<T>(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.Query<T>(commandText, param, transaction);
		}

		public virtual int Execute(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.Execute(commandText, param, transaction);
		}

		public virtual IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.ExecuteReader(commandText, param, transaction);
		}

		public virtual T ExecuteScalar<T>(string commandText, object param = null, IDbTransaction transaction = null)
		{
			return this.Connection.ExecuteScalar<T>(commandText, param, transaction);
		}

		public virtual bool OpenTransaction()
		{
			transaction = connection.BeginTransaction();
			return (transaction != null);
		}

		public virtual void CommitTransaction()
		{
			transaction.Commit();
			transaction.Dispose();
			transaction = null;
		}

		public virtual void RollbackTransaction()
		{
			transaction.Rollback();
			transaction.Dispose();
			transaction = null;
		}

		public virtual DataTable GetSchema(string schemaName)
		{
			return ((System.Data.Common.DbConnection)this.Connection).GetSchema(schemaName);
		}

		public virtual TableInfo GetTableInfo(string tableName)
		{
			return new TableInfo
			{
				Name = tableName,
				Columns = GetTableColumns(tableName)
			};
		}

		protected virtual List<ColumnInfo> GetTableColumns(string tableName)
		{
			var result = new List<ColumnInfo>();
			string query = string.Format("SELECT * FROM {0} WHERE 1 = 2", tableName);

			var da = dbProviderFactory.CreateDataAdapter();

			using (var command = this.GetCommand(query))
			{
				da.SelectCommand = (command as DbCommand);
				da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

				var dt = new DataTable();
				da.FillSchema(dt, SchemaType.Source);

				foreach (DataColumn col in dt.Columns)
				{
					var columnInfo = new ColumnInfo()
					{
						Name = col.ColumnName
					};

					columnInfo.IsAutoIncrement = col.AutoIncrement;
					columnInfo.IsPrimaryKey = dt.PrimaryKey.Any(c => c.ColumnName == col.ColumnName);
					columnInfo.IsNullable = col.AllowDBNull;

					result.Add(columnInfo);
				}
			}

			da.Dispose();

			return result;
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
