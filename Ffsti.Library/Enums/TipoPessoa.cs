using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Ffsti.Enums
{
    /// <summary>
    /// Define o tipo de pessoa
    /// </summary>
    public enum TipoPessoa
    {
        /// <summary>
        /// Pessoa Física
        /// </summary>
        [Description("Física")]
        Fisica = 1,

        /// <summary>
        /// Pessoa Jurídica
        /// </summary>
        [Description("Jurídica")]
        Juridica = 2
    }
}
