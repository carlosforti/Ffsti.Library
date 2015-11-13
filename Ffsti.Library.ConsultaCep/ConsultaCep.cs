using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Ffsti.Library.ConsultaCep.Models;

namespace Ffsti.Library.ConsultaCep
{
    public class ConsultaCep
    {
        private const string URL = "http://m.correios.com.br/movel/buscaCepConfirma.do";
        private WebRequest request;

        public Endereco PesquisaCep(string cep)
        {
            InitializeRequest(cep);
            var document = NSoup.NSoupClient.Parse(GetPageContent());

            if (!document.Select(".erro").IsEmpty)
            {
                return new Endereco()
                {
                    TipoCep = TipoCep.NaoEncontrado
                };
            }

            var div = document.Select(".respostadestaque");
            Endereco address;

            if (div.Eq(2).HasText) //O CEP é completo
            {
                var typeOfStreet = div.Eq(0).Text.Trim().Split(' ')[0];

                var streetNode = div.Eq(0).Text.Trim().Split(' ');

                var street = string.Empty;
                for (var i = 0; i < streetNode.Length; i++)
                {
                    if (i <= 0) continue;
                    if (streetNode[i] == "-") break;
                    street += streetNode[i];
                    street += " ";
                }
                street = street.Trim();

                var neighborHood = div.Eq(1).Text.Trim();
                var city = div.Eq(2).Text.Trim().Split('/')[0].Trim();
                var estate = div.Eq(2).Text.Trim().Split('/')[1].Trim();

                address = new Endereco()
                {
                    Cep = cep,
                    TipoLogradouro = typeOfStreet,
                    Logradouro = street,
                    Bairro = neighborHood,
                    Cidade = city,
                    Estado = estate,
                    TipoCep = TipoCep.CepCompleto
                };
            }
            else //O CEP é único na cidade toda
            {
                var city = div.Eq(0).Text.Trim().Split('/')[0].Trim();
                var estate = div.Eq(0).Text.Trim().Split('/')[1].Trim();

                address = new Endereco()
                {
                    Cep = cep,
                    TipoLogradouro = string.Empty,
                    Logradouro = string.Empty,
                    Bairro = string.Empty,
                    Cidade = city,
                    Estado = estate,
                    TipoCep = TipoCep.CepUnico
                };
            }

            return address;
        }

        private string GetPageContent()
        {
            var response = request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("ISO-8859-1")))
                        return reader.ReadToEnd();
                throw new EndOfStreamException();
            }
        }

        private void InitializeRequest(string cep)
        {
            this.request = WebRequest.Create(URL);

            this.request.ContentType = "application/x-www-form-urlencoded";
            this.request.Headers.Set(HttpRequestHeader.ContentEncoding, "ISO-8859-1");
            this.request.Method = "POST";
            var requestParams = Encoding.ASCII.GetBytes(string.Format("cepEntrada={0}&tipoCep=&cepTemp&metodo=buscarCep", cep));
            this.request.ContentLength = requestParams.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(requestParams, 0, requestParams.Length);
            requestStream.Close();
        }
    }
}
