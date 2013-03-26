using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ffsti.Library.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private Ffsti.Library.Database.Db db;

        [TestMethod]
        public void OpenConnection()
        {
            string connectionString = @"Data Source=XE;User Id=DESENV_CHECKLIST;Password=GA;Integrated Security=no;";
            string providerName = "System.Data.OracleClient";

            db = new Ffsti.Library.Database.Db(connectionString, providerName);

            db.OpenConnection();

            Assert.IsTrue(db.State == System.Data.ConnectionState.Open);

            System.Data.DataTable table = db.GetDataTable("SELECT * FROM ALL_USERS");

            Assert.IsTrue(table.Rows.Count > 0);

            string insert = "INSERT INTO GA_CHECK_LIST (CODIGO, DESCRICAO, NIVEL, PLATAFORMA) VALUES (@Codigo, @Descricao, @Nivel, @Plataforma)";

            System.Data.IDbCommand comm = db.GetCommand(insert);
            comm.Parameters.Add(db.CreateParameter(comm, "@Codigo", "9", System.Data.DbType.String));
            comm.Parameters.Add(db.CreateParameter(comm, "@Descricao", "Teste", System.Data.DbType.String));
            comm.Parameters.Add(db.CreateParameter(comm, "@Nivel", 1, System.Data.DbType.Int32));
            comm.Parameters.Add(db.CreateParameter(comm, "@Plataforma", 0, System.Data.DbType.Int32));

            Assert.IsTrue(comm.ExecuteNonQuery() == 1);
        }
    }
}
