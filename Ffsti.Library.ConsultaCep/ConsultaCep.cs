using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Ffsti.Library.ConsultaCep
{
    public struct Cep
    {
        public string CepPesquisado;
        public int Resultado;
        public string ResultadoTxt;
        public string Uf;
        public string Cidade;
        public string Bairro;
        public string TipoLogradouro;
        public string Logradouro;

        public override string ToString()
        {
            return CepPesquisado + " / " + Resultado + " / " + ResultadoTxt + " / " + Uf + " / " + Cidade + " / " + Bairro + " / " + TipoLogradouro + " / " + Logradouro;
        }
    }

    public static class ConsultaCep
    {
        private static string WEBSERVICE_URL_BASE = "http://webservice.kinghost.net/web_cep.php?auth={0}&formato=xml&cep={1}";

        public static Cep GetCep(string authKey, string cep)
        {
            string url = string.Format(WEBSERVICE_URL_BASE, authKey, cep);
            //XmlTextReader reader = new XmlTextReader(url);
            XDocument doc = XDocument.Load(url);

            Cep cepRetorno = new Cep();

            cepRetorno.CepPesquisado = cep;

            cepRetorno.Resultado = (from d in doc.Descendants("webservicecep")
                                    select (int)d.Element("resultado")).FirstOrDefault();

            cepRetorno.ResultadoTxt = (from d in doc.Descendants("webservicecep")
                                       select (string)d.Element("resultado_txt")).FirstOrDefault();

            cepRetorno.Uf = (from d in doc.Descendants("webservicecep")
                             select (string)d.Element("uf")).FirstOrDefault();

            cepRetorno.Cidade = (from d in doc.Descendants("webservicecep")
                                 select (string)d.Element("cidade")).FirstOrDefault();

            cepRetorno.Bairro = (from d in doc.Descendants("webservicecep")
                                 select (string)d.Element("bairro")).FirstOrDefault();

            cepRetorno.TipoLogradouro = (from d in doc.Descendants("webservicecep")
                                         select (string)d.Element("tipo_logradouro")).FirstOrDefault();

            cepRetorno.Logradouro = (from d in doc.Descendants("webservicecep")
                                     select (string)d.Element("logradouro")).FirstOrDefault();

            return cepRetorno;
        }
    }
}
