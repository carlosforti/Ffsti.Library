using Ffsti.Library.ExtensionMethods;

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Ffsti
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class StringExtensionMethods
    {
        #region String Utilities
        /// <summary>
        /// <para>Returns a phonetized string, baseado no algoritmo BuscaBr disponível em :</para>
        /// <para>http://www.unibratec.com.br/jornadacientifica/diretorio/NOVOB.pdf</para>
        /// </summary>
        /// <param name="text">Original Text</param>
        /// <param name="keepVowels">Keep or remove vowels</param>
        /// <returns>The phonetized string</returns>
        public static string Phonetized(this string text, bool keepVowels = true)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var sb = new StringBuilder(text.Trim().ToUpper().RemovePunctuation(false).RemoveAccents());

            sb.Use(s =>
            {
                s.Replace("Y", "I");
                s.Replace("BR", "B").Replace("BL", "B").Replace("PH", "F");
                s.Replace("GR", "G").Replace("GL", "G").Replace("MG", "G");
                s.Replace("NG", "G").Replace("RG", "G").Replace("GE", "J");
                s.Replace("GI", "J").Replace("RJ", "J").Replace("MJ", "J");
                s.Replace("NJ", "J").Replace("CE", "S").Replace("CI", "S");
                s.Replace("CH", "S").Replace("CT", "T").Replace("CS", "S");
                s.Replace("Q", "K").Replace("CA", "K").Replace("CO", "K");
                s.Replace("CU", "K").Replace("C", "K").Replace("LH", "H");
                s.Replace("RM", "SM").Replace("N", "M").Replace("GM", "M");
                s.Replace("MD", "M").Replace("NH", "N").Replace("PR", "P");
                s.Replace("X", "S").Replace("TS", "S").Replace("C", "S");
                s.Replace("Z", "S").Replace("RS", "S").Replace("TR", "T");
                s.Replace("TL", "T").Replace("LT", "T").Replace("RT", "T");
                s.Replace("ST", "T").Replace("W", "V");
            });

            var tam = sb.Length - 1;
            if (tam > -1 &&
                (sb[tam] == 'S' || sb[tam] == 'Z' || sb[tam] == 'R' || sb[tam] == 'M' || sb[tam] == 'N' ||
                 sb[tam] == 'L'))
            {
                sb.Remove(tam, 1);
            }
            tam = sb.Length - 2;

            if (tam > -1 && sb[tam] == 'A' && sb[tam + 1] == 'O')
            {
                sb.Remove(tam, 2);
            }

            sb.Replace("Ç", "S").Replace("L", "R");

            if (!keepVowels)
                sb.Replace("A", "").Replace("E", "").Replace("I", "").Replace("O", "").Replace("U", "");
            sb.Replace("H", "");

            var frasesaida = new StringBuilder();

            if (sb.Length <= 0)
                return frasesaida.ToString();

            frasesaida.Append(sb[0]);
            for (var i = 1; i <= sb.Length - 1; i += 1)
                if (frasesaida[frasesaida.Length - 1] != sb[i] || char.IsDigit(sb[i]))
                    frasesaida.Append(sb[i]);

            return frasesaida.ToString();
        }

        /// <summary>
        /// Removes the acents from text
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="includeCedilla">Remove the cedillas (ç), replacing then with c's</param>
        /// <returns>Accent-free text</returns>
        public static string RemoveAccents(this string text, bool includeCedilla = true)
        {

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            text = Regex.Replace(text, "[áàãâä]", "a");
            text = Regex.Replace(text, "[ÁÀÃÂÄ]", "A");
            text = Regex.Replace(text, "[èéêë]", "e");
            text = Regex.Replace(text, "[ÈÉÊË]", "E");
            text = Regex.Replace(text, "[îìïí]", "i");
            text = Regex.Replace(text, "[ÎÌÏÍ]", "I");
            text = Regex.Replace(text, "[óòõöô]", "o");
            text = Regex.Replace(text, "[ÓÒÕÖÔ]", "O");
            text = Regex.Replace(text, "[úùüû]", "u");
            text = Regex.Replace(text, "[ÚÙÜÛ]", "U");

            if (includeCedilla)
                text = text.Replace("ç", "c").Replace("Ç", "C");

            return text;
        }

        /// <summary>
        /// Removes the punctuation from text
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="removeSpaces">Remove the spaces inside the string</param>
        /// <returns>Punctuation-free text</returns>
        public static string RemovePunctuation(this string text, bool removeSpaces = true)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            text = text.Trim();
            text = Regex.Replace(text, @"[-_/\.,:;()]", string.Empty);

            if (removeSpaces)
                text = text.Replace(" ", string.Empty);

            return text;
        }

        /// <summary>
        /// Compress a string using a GZipStream
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Compressed text</returns>
        public static string Compress(this string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);

            var memoryStream = new MemoryStream();
            var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

            gZipStream.Write(buffer, 0, buffer.Length);

            memoryStream.Position = 0;

            var compressed = new byte[memoryStream.Length];
            memoryStream.Read(compressed, 0, compressed.Length);

            var gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// Decompress a string using a GZipStream
        /// </summary>
        /// <param name="compressedText">Original compressed text</param>
        /// <returns>Decompressed text</returns>
        public static string Decompress(this string compressedText)
        {
            var gzBuffer = Convert.FromBase64String(compressedText);

            var memoryStream = new MemoryStream();

            var msgLength = BitConverter.ToInt32(gzBuffer, 0);
            memoryStream.Write(gzBuffer, 4, gzBuffer.Length - 4);

            var buffer = new byte[msgLength];

            memoryStream.Position = 0;
            var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            gZipStream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }
        #endregion

        #region Brazilian documents - CPF and CNPJ
        /// <summary>
        /// Format a string as a CPF (Brazilian personal document)
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>CPF formatted text</returns>
        public static string FormatCpf(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.RemovePunctuation();
            if (text.Length < 11) text = text.PadLeft(11, '0');
            return string.Format("{0:000}.{1:000}.{2:000}-{3:00}",
                text.Substring(0, 3),
                text.Substring(3, 3),
                text.Substring(6, 3),
                text.Substring(9, 2));
        }

        /// <summary>
        /// Format a string as a CNPJ (Brazilian company document)
        /// </summary>
        /// <param name="text">Original Text</param>
        /// <returns>CNPJ formatted text</returns>
        public static string FormatCnpj(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.RemovePunctuation();
            return
                $"{text.Substring(0, 2)}.{text.Substring(2, 3)}.{text.Substring(5, 3)}/{text.Substring(8, 4)}-{text.Substring(12, 2)}";
        }

        /// <summary>
        /// Format a string as a CPF (Brazilian personal document) or CNPJ (Brazilian company document), base in text size
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>CPF or CNPJ formatted text</returns>
        public static string FormatBrazilianDocument(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.RemovePunctuation();
            if (text.Length < 14)
                return FormatCpf(text);
            return FormatCnpj(text);
        }

        /// <summary>
        /// Validates a CNPJ (Brazilian company document)
        /// </summary>
        /// <param name="cnpj">CNPJ to be validated</param>
        /// <returns>If it is a valid CNPJ, returns true, else returns false</returns>
        public static bool ValidateCnpj(this string cnpj)
        {
            var cnpjOriginal = cnpj.RemovePunctuation();
            if (!long.TryParse(cnpjOriginal, out _)) return false;
            if (cnpjOriginal.Length != 14) return false;

            var cnpjComparacao = cnpjOriginal.Substring(0, 12);
            var charCnpj = cnpjOriginal.ToCharArray();

            /* Primeira parte */
            var soma = CalcularPrimeiraParteCnpj(charCnpj);
            cnpjComparacao += CalculaDigito(0);

            /* Segunda parte */
            soma = CalcularSegundaParteCnpj(charCnpj);
            cnpjComparacao += CalculaDigito(0);

            return cnpjOriginal == cnpjComparacao;
        }

        private static int CalcularSegundaParteCnpj(char[] charCnpj)
        {
            var soma = 0;

            for (var i = 0; i < 5; i++)
                    soma += (charCnpj[i] - 48) * (7 - (i + 1));

            for (var i = 0; i < 8; i++)
                    soma += (charCnpj[i + 5] - 48) * (10 - (i + 1));

            return soma;
        }

        private static int CalcularPrimeiraParteCnpj(char[] charCnpj)
        {
            var soma = 0;
            for (var i = 0; i < 4; i++)
                    soma += (charCnpj[i] - 48) * (6 - (i + 1));

            for (var i = 0; i < 8; i++)
                    soma += (charCnpj[i + 4] - 48) * (10 - (i + 1));

            return soma;
        }

        private static string CalculaDigito(int soma)
        {
            var dig = 11 - soma % 11;
            return dig == 10 || dig == 11 ? "0" : dig.ToString();
        }

        /// <summary>
        /// Validates a CPF (Brazilian personal document)
        /// </summary>
        /// <param name="cpf">CPF to be validated</param>
        /// <returns>If it is a valid CPF, returns true, else returns false</returns>
        public static bool ValidateCpf(this string cpf)
        {
            //variáveis
            int digito1, digito2;
            var adicao = 0;

            var cpfComparacao = cpf.RemovePunctuation();

            // Se o tamanho for < 11 entao retorna como inválido
            if (cpfComparacao.Length != 11) return false;

            // Pesos para calcular o primeiro número 
            var array1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Pesos para calcular o segundo número
            var array2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Caso coloque todos os numeros iguais
            if (Regex.Match(cpfComparacao, "0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11}").Success) return false;

            // Calcula cada número com seu respectivo peso 
            for (var i = 0; i <= array1.GetUpperBound(0); i++)
                adicao += array1[i] * Convert.ToInt32(cpfComparacao[i].ToString());

            // Pega o resto da divisão 
            var resto = adicao % 11;

            if (resto == 1 || resto == 0)
                digito1 = 0;
            else
                digito1 = 11 - resto;

            adicao = 0;

            // Calcula cada número com seu respectivo peso 
            for (var i = 0; i <= array2.GetUpperBound(0); i++)
                adicao += array2[i] * Convert.ToInt32(cpfComparacao[i].ToString());

            // Pega o resto da divisão 
            resto = adicao % 11;

            if (resto == 1 || resto == 0)
                digito2 = 0;
            else
                digito2 = 11 - resto;

            var calculo = digito1.ToString() + digito2.ToString();
            var digito = cpfComparacao.Substring(9, 2);

            // Se os ultimos dois digitos calculados bater com 
            // os dois ultimos digitos do cpf entao é válido 
            return calculo == digito;
        }

        /// <summary>
        /// Validates a CPF (Brazilian personal document) or CNPJ (Brazilian company document)
        /// </summary>
        /// <param name="document">CPF or CNPJ to be validated</param>
        /// <returns>If it is a valid CPF or CNPJ, returns true, else returns false</returns>
        public static bool ValidateBrazilianDocument(this string document)
        {
            document = document.RemovePunctuation();

            if (document.Length == 11) return document.ValidateCpf();
            if (document.Length == 14) return document.ValidateCnpj();
            return false;
        }
        #endregion

        /// <summary>
        /// <para>Format a telephone number using Brazilan formatting</para>
        /// <para>This method respects the 9th digit in cell phone numbers</para>
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Phone formatted text</returns>
        public static string FormatPhoneNumber(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var longDistancePrefix = "";
            var countryPrefix = "";

            if (text.Contains("+"))
            {
                var countryPrefixStart = text.IndexOf("+", StringComparison.Ordinal);
                var countryPrefixEnd = text.IndexOf(" ", countryPrefixStart, StringComparison.Ordinal);

                countryPrefix = text.Substring(countryPrefixStart, countryPrefixEnd - countryPrefixStart).Trim();

                text = text.Replace(countryPrefix, "").Trim();
            }

            if ((text.Contains("-") && text.Length > 10) || (text.Length > 9))
            {
                var prefixStart = 0;

                if (text.Contains("("))
                    prefixStart = text.IndexOf("(", StringComparison.Ordinal);

                int prefixEnd;
                if (text.Contains(")"))
                    prefixEnd = text.IndexOf(")", StringComparison.Ordinal);
                else if (text.Contains(" "))
                    prefixEnd = text.IndexOf(" ", prefixStart + 1, StringComparison.Ordinal);
                else //Usará sempre o padrão Brasil (2 digito)
                    prefixEnd = prefixStart + 2;

                longDistancePrefix =
                    $"({text.Substring(prefixStart, prefixEnd - prefixStart).RemovePunctuation().Trim()})";
                var realPrefix = text.Substring(prefixStart, prefixEnd - prefixStart);

                text = text.Replace(realPrefix, "").Trim();
            }

            text = text.RemovePunctuation();
            var leftDigits = text.Length - 4;
            var begin = text.Substring(0, leftDigits);
            var end = text.Substring(leftDigits);
            
            string mainNumber = $"{begin}-{end}";

            return $"{countryPrefix} {longDistancePrefix} {mainNumber}".Trim();
        }

        /// <summary>
        /// Format a Brazilian ZIP Code (99999-999)
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Brazilian ZIP code formatted text</returns>
        public static string FormatBrazilianZipCode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return text.Length < 8
                ? text
                : $"{text.Substring(0, 5)}-{text.Substring(5, 3)}";
        }

        /// <summary>
        /// Convert a string to a nullable decimal value
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Nullable Decimal</returns>
        public static decimal? ToDecimalOrNull(this string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            decimal valor;
            return decimal.TryParse(text.Replace(separador, string.Empty), out valor)
                ? (decimal?)valor
                : null;
        }

        /// <summary>
        /// Convert a string to a decimal
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <returns>Decimal from the text</returns>
        /// <exception cref="FormatException">If the string is not a valid date/time, throws the FormatException</exception>
        public static decimal ToDecimal(this string text)
        {
            var result = text.ToDecimalOrNull();

            if (result == null)
                throw new FormatException();

            return result.Value;
        }

        /// <summary>
        /// Convert a string as a currency text
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Currency text</returns>
        /// <exception cref="FormatException">If the string is not a valid currency, throws the FormatException</exception>
        public static string ToCurrency(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var valor = text.ToDecimal();
            return
                valor.ToString("c" + System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        }

        /// <summary>
        /// Convert a string to a nullable integer
        /// </summary>
        /// <param name="text">Original Text</param>
        /// <returns>Nullable integer</returns>
        public static int? ToIntOrNull(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                int valor;
                if (int.TryParse(text, out valor))
                    return valor;
                return null;
            }
            return null;
        }

        /// <summary>
        /// Convert a string to an integer
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <returns>Integer from the text</returns>
        /// <exception cref="FormatException">If the string is not a valid integer, throws the FormatException</exception>
        public static int ToInt(this string text)
        {
            var result = text.ToIntOrNull();

            if (result == null)
            {
                throw new FormatException();
            }

            return result.Value;

        }

        /// <summary>
        /// Convert a string to a nullable date/time
        /// </summary>
        /// <param name="text">Original Text</param>
        /// <returns>Nullable date/tim</returns>
        public static DateTime? ToDateTimeOrNull(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                DateTime valor;
                if (DateTime.TryParse(text, out valor))
                    return valor;
                return null;
            }
            return null;
        }

        /// <summary>
        /// Convert a string to a date/time.
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <returns>DateTime from the text</returns>
        /// <exception cref="FormatException">If the string is not a valid date/time, throws the FormatException</exception>
        public static DateTime ToDateTime(this string text)
        {
            var result = text.ToDateTimeOrNull();

            if (result == null)
            {
                throw new FormatException();
            }
            return result.Value;
        }

        /// <summary>
        /// Returns the string trimmed and upper case
        /// </summary>
        /// <returns></returns>
        public static string TrimAndUpper(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text.Trim().ToUpper();
        }
    }
}