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
				throw new ArgumentNullException("text");

			//Devemos retirar os caracteres especiais e converter todas as letras para Maiúsculo
			//Eliminamos todos os acentos
			StringBuilder sb = new StringBuilder(text.Trim().ToUpper().RemovePunctuation(false));

			//Substituimos
			sb.Replace("Y", "I");
			sb.Replace("BR", "B").Replace("BL", "B").Replace("PH", "F");
			sb.Replace("GR", "G").Replace("GL", "G").Replace("MG", "G");
			sb.Replace("NG", "G").Replace("RG", "G").Replace("GE", "J");
			sb.Replace("GI", "J").Replace("RJ", "J").Replace("MJ", "J");
			sb.Replace("NJ", "J").Replace("CE", "S").Replace("CI", "S");
			sb.Replace("CH", "S").Replace("CT", "T").Replace("CS", "S");
			sb.Replace("Q", "K").Replace("CA", "K").Replace("CO", "K");
			sb.Replace("CU", "K").Replace("C", "K").Replace("LH", "H");
			sb.Replace("RM", "SM").Replace("N", "M").Replace("GM", "M");
			sb.Replace("MD", "M").Replace("NH", "N").Replace("PR", "P");
			sb.Replace("X", "S").Replace("TS", "S").Replace("C", "S");
			sb.Replace("Z", "S").Replace("RS", "S").Replace("TR", "T");
			sb.Replace("TL", "T").Replace("LT", "T").Replace("RT", "T");
			sb.Replace("ST", "T").Replace("W", "V");

			//Eliminamos as terminações S, Z, R, R, M, N, AO e L
			int tam = sb.Length - 1;
			if (tam > -1)
				if (sb[tam] == 'S' || sb[tam] == 'Z' || sb[tam] == 'R' || sb[tam] == 'M' || sb[tam] == 'N' || sb[tam] == 'L')
					sb.Remove(tam, 1);
			tam = sb.Length - 2;

			if (tam > -1)
				if (sb[tam] == 'A' && sb[tam + 1] == 'O')
					sb.Remove(tam, 2);

			//Substituimos L por R e Ç por S;
			sb.Replace("Ç", "S").Replace("L", "R");

			//O BuscaBr diz para eliminamos todas as vogais e o H, 
			//porém ao implementar notamos que não seria necessário 
			//eliminarmos as vogais, isso dificultaria muito a busca dos dados "pós BuscaBR"
			if (!keepVowels)
				sb.Replace("A", "").Replace("E", "").Replace("I", "").Replace("O", "").Replace("U", "");
			sb.Replace("H", "");

			//Eliminamos todas as letras em duplicidade;
			StringBuilder frasesaida = new StringBuilder();
			if (sb.Length > 0)
			{
				frasesaida.Append(sb[0]);
				for (int i = 1; i <= sb.Length - 1; i += 1)
					if (frasesaida[frasesaida.Length - 1] != sb[i] || char.IsDigit(sb[i]))
						frasesaida.Append(sb[i]);
			}

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

				throw new ArgumentNullException("text");

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
				throw new ArgumentNullException("text");
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
			byte[] buffer = Encoding.UTF8.GetBytes(text);

			MemoryStream memoryStream = new MemoryStream();
			GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

			gZipStream.Write(buffer, 0, buffer.Length);

			memoryStream.Position = 0;

			byte[] compressed = new byte[memoryStream.Length];
			memoryStream.Read(compressed, 0, compressed.Length);

			byte[] gzBuffer = new byte[compressed.Length + 4];
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
			byte[] gzBuffer = Convert.FromBase64String(compressedText);

			MemoryStream memoryStream = new MemoryStream();

			int msgLength = BitConverter.ToInt32(gzBuffer, 0);
			memoryStream.Write(gzBuffer, 4, gzBuffer.Length - 4);

			byte[] buffer = new byte[msgLength];

			memoryStream.Position = 0;
			GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
			gZipStream.Read(buffer, 0, buffer.Length);

			return Encoding.UTF8.GetString(buffer);
		}

		/// <summary>
		/// Format a string as a CPF (Brazilian personal document)
		/// </summary>
		/// <param name="text">Original text</param>
		/// <returns>CPF formatted text</returns>
		public static string FormatCpf(this string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			else
			{
				text = text.RemovePunctuation();
				if (text.Length < 11) text = text.PadLeft(11, '0');
				return string.Format("{0:3}.{1}.{2}-{3}",
					text.Substring(0, 3), text.Substring(3, 3), text.Substring(6, 3), text.Substring(9, 2));
			}
		}

		/// <summary>
		/// Format a string as a CNPJ (Brazilian company document)
		/// </summary>
		/// <param name="text">Original Text</param>
		/// <returns>CNPJ formatted text</returns>
		public static string FormatCnpj(this string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			else
			{
				text = text.RemovePunctuation();
				return string.Format("{0}.{1}.{2}/{3}-{4}",
					text.Substring(0, 2), text.Substring(2, 3), text.Substring(5, 3), text.Substring(8, 4), text.Substring(12, 2));
			}
		}

		/// <summary>
		/// Format a string as a CPF (Brazilian personal document) or CNPJ (Brazilian company document), base in text size
		/// </summary>
		/// <param name="text">Original text</param>
		/// <returns>CPF or CNPJ formatted text</returns>
		public static string FormatBrazilianDocument(this string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			else
			{
				text = text.RemovePunctuation();
				if (text.Length < 14)
					return FormatCpf(text);
				else
					return FormatCnpj(text);
			}
		}

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
			else
			{
				string aux = text;
				string mainNumber = "";
				string longDistancePrefix = "";
				string countryPrefix = "";

				if (text.Contains("+"))
				{
					int countryPrefixStart = text.IndexOf("+");
					int countryPrefixEnd = text.IndexOf(" ", countryPrefixStart);

					countryPrefix = text.Substring(countryPrefixStart, countryPrefixEnd - countryPrefixStart).Trim();

					text = text.Replace(countryPrefix, "").Trim();
				}

				if ((text.Contains("-") && text.Length > 10) || (text.Length > 9))
				{
					int prefixStart = 0;
					int prefixEnd = 0;

					if (text.Contains("("))
						prefixStart = text.IndexOf("(");

					if (text.Contains(")"))
						prefixEnd = text.IndexOf(")");
					else if (text.Contains(" "))
						prefixEnd = text.IndexOf(" ", prefixStart + 1);
					else //Usará sempre o padrão Brasil (2 digito)
						prefixEnd = prefixStart + 2;

					longDistancePrefix = string.Format("({0})", text.Substring(prefixStart, prefixEnd - prefixStart).RemovePunctuation().Trim());
					string realPrefix = text.Substring(prefixStart, prefixEnd - prefixStart);

					text = text.Replace(realPrefix, "").Trim();
				}

				string begin = "";
				string end = "";

				text = text.RemovePunctuation(true);
				//if (text.Contains("-") || text.Contains(" "))
				//{
				//	begin = text.Substring(0, text.IndexOf("-"));
				//	end = text.Substring(text.IndexOf("-") + 1);
				//}
				//else
				//{
				int leftDigits = text.Length - 4;
				begin = text.Substring(0, leftDigits);
				end = text.Substring(leftDigits);
				//}

				mainNumber = string.Format("{0}-{1}", begin, end);

				return string.Format("{0} {1} {2}", countryPrefix, longDistancePrefix, mainNumber).Trim();
			}
		}

		/// <summary>
		/// Format a Brazilian ZIP Code (99999-999)
		/// </summary>
		/// <param name="text">Original text</param>
		/// <returns>Brazilian ZIP code formatted text</returns>
		public static string FormatBrazilianZipCode(this string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			else
			{
				if (text.Length < 8) return text;
				else return string.Format("{0}-{1}", text.Substring(0, 5), text.Substring(5, 3));
			}
		}

		/// <summary>
		/// Convert a string to a nullable decimal value
		/// </summary>
		/// <param name="text">Original text</param>
		/// <returns>Nullable Decimal</returns>
		public static decimal? ToDecimal(this string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				var separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
				decimal valor;
				if (decimal.TryParse(text.Replace(separador, string.Empty), out valor))
					return valor;
				else
					return null;
			}
			else return null;
		}

		/// <summary>
		/// Convert a string as a currency text
		/// </summary>
		/// <param name="text">Original text</param>
		/// <returns>Currency text</returns>
		public static string ToCurrency(this string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			else
			{
				decimal? valor = ToDecimal(text);
				if (valor.HasValue)
				{
					return valor.Value.ToString("c" + System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
				}
				else return string.Empty;
			}
		}

		/// <summary>
		/// Convert a string to a nullable integer
		/// </summary>
		/// <param name="text">Original Text</param>
		/// <returns>Nullable integer</returns>
		public static int? ToInt(this string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				int valor;
				if (int.TryParse(text, out valor))
					return valor;
				else
					return null;
			}
			else
				return null;
		}

		/// <summary>
		/// Convert a string to a nullable date/time
		/// </summary>
		/// <param name="text">Original Text</param>
		/// <returns>Nullable date/tim</returns>
		public static DateTime? ToDateTime(this string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				DateTime valor;
				if (DateTime.TryParse(text, out valor))
					return valor;
				else
					return null;
			}
			else return null;
		}

		/// <summary>
		/// Validates a CNPJ (Brazilian company document)
		/// </summary>
		/// <param name="cnpj">CNPJ to be validated</param>
		/// <returns>If it is a valid CNPJ, returns true, else returns false</returns>
		public static bool ValidateCnpj(this string cnpj)
		{
			int soma = 0, dig;

			string cnpjOriginal = cnpj.RemovePunctuation();
			if (cnpjOriginal.Length != 14) return false;

			string cnpjComparacao = cnpjOriginal.Substring(0, 12);

			char[] charCnpj = cnpjOriginal.ToCharArray();

			/* Primeira parte */
			for (int i = 0; i < 4; i++)
				if (charCnpj[i] - 48 >= 0 && charCnpj[i] - 48 <= 9)
					soma += (charCnpj[i] - 48) * (6 - (i + 1));
			for (int i = 0; i < 8; i++)
				if (charCnpj[i + 4] - 48 >= 0 && charCnpj[i + 4] - 48 <= 9)
					soma += (charCnpj[i + 4] - 48) * (10 - (i + 1));
			dig = 11 - (soma % 11);

			cnpjComparacao += (dig == 10 || dig == 11) ? "0" : dig.ToString();

			/* Segunda parte */
			soma = 0;
			for (int i = 0; i < 5; i++)
				if (charCnpj[i] - 48 >= 0 && charCnpj[i] - 48 <= 9)
					soma += (charCnpj[i] - 48) * (7 - (i + 1));
			for (int i = 0; i < 8; i++)
				if (charCnpj[i + 5] - 48 >= 0 && charCnpj[i + 5] - 48 <= 9)
					soma += (charCnpj[i + 5] - 48) * (10 - (i + 1));
			dig = 11 - (soma % 11);
			cnpjComparacao += (dig == 10 || dig == 11) ? "0" : dig.ToString();

			return cnpjOriginal == cnpjComparacao;
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
			int adicao = 0;

			string digito = "";
			string calculo = "";

			string cpfComparacao = cpf.RemovePunctuation();

			// Se o tamanho for < 11 entao retorna como inválido
			if (cpfComparacao.Length != 11) return false;

			// Pesos para calcular o primeiro número 
			int[] array1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			// Pesos para calcular o segundo número
			int[] array2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			// Caso coloque todos os numeros iguais
			if (Regex.Match(cpfComparacao, "0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11}").Success) return false;

			// Calcula cada número com seu respectivo peso 
			for (int i = 0; i <= array1.GetUpperBound(0); i++)
				adicao += (array1[i] * Convert.ToInt32(cpfComparacao[i].ToString()));

			// Pega o resto da divisão 
			int resto = adicao % 11;

			if (resto == 1 || resto == 0)
				digito1 = 0;
			else
				digito1 = 11 - resto;

			adicao = 0;

			// Calcula cada número com seu respectivo peso 
			for (int i = 0; i <= array2.GetUpperBound(0); i++)
				adicao += (array2[i] * Convert.ToInt32(cpfComparacao[i].ToString()));

			// Pega o resto da divisão 
			resto = adicao % 11;

			if (resto == 1 || resto == 0)
				digito2 = 0;
			else
				digito2 = 11 - resto;

			calculo = digito1.ToString() + digito2.ToString();
			digito = cpfComparacao.Substring(9, 2);

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
			else if (document.Length == 14) return document.ValidateCnpj();
			else return false;
		}

		//TODO: rever como implantar
		//public static string Encrypt(this string value)
		//{
		//    return Cryptography.Encrypt(value);
		//}

		//public static string Decrypt(this string value)
		//{
		//    return Cryptography.Decrypt(value);
		//}
	}
}