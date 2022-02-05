using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using Dapper;
using Ffsti.Library.Database.Model;

namespace Ffsti.Library.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class Db : IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private readonly string _connectionString;

        private DbProviderFactory _dbProviderFactory;

        /// <summary>
        /// 
        /// </summary>
        protected IDbConnection Connection => _connection;

        /// <summary>
        /// 
        /// </summary>
        public ConnectionState State => _connection.State;

        /// <summary>
        /// 
        /// </summary>
        public IDbTransaction Transaction => _transaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
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

        /// <summary>
        /// 
        /// </summary>
        public virtual void OpenConnection()
        {
            _connection = _dbProviderFactory.CreateConnection();
            if (_connection == null)
                return;

            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CloseConnection()
        {
            _connection.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual IDbCommand GetCommand(string commandText)
        {
            var comm = _connection.CreateCommand();
            comm.CommandText = commandText;
            if (_transaction != null)
                comm.Transaction = _transaction;

            return comm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string commandText)
        {
            return GetCommand(commandText).ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteScalar<T>(commandText, param, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string commandText)
        {
            return GetCommand(commandText).ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.Query<T>(commandText, param, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int Execute(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.Execute(commandText, param, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteReader(commandText, param, transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool OpenTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction != null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public virtual DataTable GetSchema(string schemaName)
        {
            return ((DbConnection)Connection).GetSchema(schemaName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual TableInfo GetTableInfo(string tableName)
        {
            return new TableInfo(tableName, GetTableColumns(tableName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _transaction?.Dispose();
            _dbProviderFactory = null;

            _connection?.Close();
            _connection?.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
