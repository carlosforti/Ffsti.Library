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
            Console.WriteLine(ConsultaCep.ConsultaCep.GetCep("881296b989d80fdb142000196467080d", "13416030"));
            Console.WriteLine();
            Console.WriteLine(ConsultaCep.ConsultaCep.GetCep("881296b989d80fdb142000196467080d", "13390000"));
            Console.WriteLine();
            Console.WriteLine(ConsultaCep.ConsultaCep.GetCep("881296b989d80fdb142000196467080d", "13416016"));
            Console.ReadKey();
        }
    }
}
