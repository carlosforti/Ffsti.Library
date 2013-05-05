using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ffsti.Library.Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Carlos\Documents\Visual Studio 2010\Projects\NFe\NFe\NFe\Data\NFe.mdb";
            string provider = "System.Data.OleDb";
            var db = new Ffsti.Library.Database.Db(connectionString, provider);

            db.OpenConnection();

            var comm = db.GetCommand("SELECT COUNT(1) FROM NF_TVM WHERE NF = @NF");
            var param = db.CreateParameter(comm, "@NF", "1", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            comm.Parameters.Add(param);

            Console.WriteLine(comm.ExecuteScalar());

            db.CloseConnection();

            Console.ReadKey();
        }
    }
}
