using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Ffsti.Library.Database
{
    public class Db
    {
        private IDbConnection connection = null;
        private IDbTransaction transaction = null;

        private string connectionString;
        private string providerName;

        private DbProviderFactory dbProviderFactory = null;

        public ConnectionState State
        {
            get { return this.connection.State; }
        }

        public Db(string connectionString, string providerName)
        {
            this.connectionString = connectionString;
            this.providerName = providerName;

            dbProviderFactory = DbProviderFactories.GetFactory(providerName);

            connection = dbProviderFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;
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

        public IDbCommand GetCommand(string command)
        {
            var comm = connection.CreateCommand();
            comm.CommandText = command;

            return comm;
        }

        public DataTable GetDataTable(string query)
        {
            var dataAdapter = dbProviderFactory.CreateDataAdapter();
            dataAdapter.SelectCommand = (DbCommand)GetCommand(query);
            DataTable table = new DataTable();
            dataAdapter.Fill(table);

            return table;
        }

        public bool OpenTransaction()
        {
            transaction = connection.BeginTransaction();
            return (transaction != null);
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }

        public IDataParameter CreateParameter(IDbCommand command, string parameterName, object value,
            DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            System.Data.IDataParameter param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbType;
            param.Direction = parameterDirection;

            return param;
        }
    }
}
