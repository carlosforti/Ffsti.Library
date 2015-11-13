using System;

namespace Ffsti.Library.ConsultaCep.Models
{
    public class Endereco
    {
        public String Cep { get; set; }
        public String TipoLogradouro { get; set; }
        public String Logradouro { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }
        public String Estado { get; set; }
        public TipoCep TipoCep { get; set; }
    }
}
