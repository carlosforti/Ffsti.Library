using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using Dapper;
using Ffsti.Library.Database.Model;

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

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private readonly string _connectionString;

        private DbProviderFactory _dbProviderFactory;

        protected IDbConnection Connection => _connection;

        public ConnectionState State => _connection.State;

        public IDbTransaction Transaction => _transaction;

        public Db(string connectionString, string providerName)
        {
            _connectionString = connectionString;

            _dbProviderFactory = DbProviderFactories.GetFactory(providerName);

            _connection = _dbProviderFactory.CreateConnection();
            if (_connection == null)
                return;

            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        public virtual void OpenConnection()
        {
            _connection = _dbProviderFactory.CreateConnection();
            if (_connection == null)
                return;

            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        public virtual void CloseConnection()
        {
            _connection.Close();
        }

        public virtual IDbCommand GetCommand(string commandText)
        {
            var comm = _connection.CreateCommand();
            comm.CommandText = commandText;
            if (_transaction != null)
                comm.Transaction = _transaction;

            return comm;
        }

        public virtual DataTable GetDataTable(string query)
        {
            using (var dataAdapter = _dbProviderFactory.CreateDataAdapter())
            {
                if (dataAdapter == null)
                    return null;

                dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
                var table = new DataTable();
                dataAdapter.Fill(table);

                return table;
            }
        }

        public virtual DataTable GetDataTable(string query, params IDataParameter[] parameters)
        {
            using (var dataAdapter = _dbProviderFactory.CreateDataAdapter())
            {
                if (dataAdapter == null)
                    return null;

                dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
                foreach (var t in parameters)
                {
                    dataAdapter.SelectCommand.Parameters.Add(t);
                }

                var table = new DataTable();
                dataAdapter.Fill(table);

                return table;
            }
        }

        public virtual DataTable GetDataTable(IDbCommand command)
        {
            using (var dataAdapter = _dbProviderFactory.CreateDataAdapter())
            {
                if (dataAdapter == null)
                    return null;

                dataAdapter.SelectCommand = (DbCommand)command;

                var table = new DataTable();
                dataAdapter.Fill(table);

                return table;
            }
        }

        //public virtual IDataReader ExecuteReader(string commandText)
        //{
        //    return GetCommand(commandText).ExecuteReader();
        //}

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
            return Connection.Query<T>(commandText, param, transaction);
        }

        public virtual int Execute(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.Execute(commandText, param, transaction);
        }

        public virtual IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteReader(commandText, param, transaction);
        }

        public virtual T ExecuteScalar<T>(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteScalar<T>(commandText, param, transaction);
        }

        public virtual bool OpenTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction != null;
        }

        public virtual void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public virtual void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public virtual DataTable GetSchema(string schemaName)
        {
            return ((DbConnection)Connection).GetSchema(schemaName);
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
            var query = $"SELECT * FROM {tableName} WHERE 1 = 2";

            var adapter = _dbProviderFactory.CreateDataAdapter();

            using (var command = GetCommand(query))
            {
                if (adapter != null)
                {
                    adapter.SelectCommand = command as DbCommand;
                    adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                    var dt = new DataTable();
                    adapter.FillSchema(dt, SchemaType.Source);

                    result.AddRange(from DataColumn col in dt.Columns
                                    select new ColumnInfo
                                    {
                                        Name = col.ColumnName,
                                        IsAutoIncrement = col.AutoIncrement,
                                        IsPrimaryKey = dt.PrimaryKey.Any(c => c.ColumnName == col.ColumnName),
                                        IsNullable = col.AllowDBNull
                                    });
                }
            }

            adapter?.Dispose();

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _transaction?.Dispose();
            _dbProviderFactory = null;

            if (_connection == null)
                return;

            _connection.Close();
            _connection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
