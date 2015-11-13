using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ffsti.Library.Tests
{
    [TestClass]
    public class CepTest
    {
        [TestMethod]
        public void TestCepUnico()
        {
            var consulta = new ConsultaCep.ConsultaCep();
            var result = consulta.PesquisaCep("13390000");
            Assert.IsTrue(result.TipoCep == ConsultaCep.Models.TipoCep.CepUnico);
        }

        [TestMethod]
        public void TestCepCompleto()
        {
            var consulta = new ConsultaCep.ConsultaCep();
            var result = consulta.PesquisaCep("13414018");
            Assert.IsTrue(result.TipoCep == ConsultaCep.Models.TipoCep.CepCompleto);
        }

        [TestMethod]
        public void TestCepNaoEncontrado()
        {
            var consulta = new ConsultaCep.ConsultaCep();
            var result = consulta.PesquisaCep("99999999");
            Assert.IsTrue(result.TipoCep == ConsultaCep.Models.TipoCep.NaoEncontrado);
        }
    }
}
