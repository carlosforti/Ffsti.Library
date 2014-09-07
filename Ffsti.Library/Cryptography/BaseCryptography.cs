using System;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti.Library.Cryptography
{
	/// <summary>
	/// Base class for cryptography.
	/// </summary>
	/// <typeparam name="T">A SymmetricAlgorithm derivated class (ie.: AesManaged)</typeparam>
	public class BaseCryptography<T>
		where T : SymmetricAlgorithm, new()
	{
		#region Fields
		private SymmetricAlgorithm algorithm = new T();
		private string key = "";

		/// <summary>
		/// Gets or sets the secret key to the algorithm
		/// </summary>
		public string Key
		{
			get { return key; }
			set { key = value; }
		}
		#endregion

		#region Private Methods
		private byte[] MergeIVAndData(byte[] iv, byte[] data)
		{
			var result = new byte[iv.Length + data.Length];

			for (int i = 0; i < iv.Length; i++)
			{
				result[i] = iv[i];
			}

			for (int i = 0; i < data.Length; i++)
			{
				result[i + iv.Length] = data[i];
			}

			return result;
		}

		private byte[] GetIV(byte[] array, int ivLength)
		{
			var result = new byte[ivLength];

			for (int i = 0; i < ivLength; i++)
			{
				result[i] = array[i];
			}

			return result;
		}

		private byte[] GetData(byte[] array, int ivLength)
		{
			var dataLength = array.Length - ivLength;

			var result = new byte[dataLength];

			for (int i = 0; i < dataLength; i++)
			{
				result[i] = array[i + ivLength];
			}

			return result;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates the Cryptography class using the given secret key
		/// </summary>
		/// <param name="secretKey">Base64 secret key</param>
		public BaseCryptography(string secretKey)
		{
			algorithm = (SymmetricAlgorithm)Activator.CreateInstance(algorithmType);
			key = secretKey;
		}

		/// <summary>
		/// Creates the Cryptography class using the given secret key
		/// </summary>
		/// <param name="secretKey">Secret key as a byte array</param>
		public BaseCryptography(byte[] secretKey)
		{
			key = Convert.ToBase64String(secretKey);
		}

		/// <summary>
		/// Creates the Cryptography class using a random generated secret key
		/// </summary>
		public BaseCryptography()
		{
			algorithm.GenerateKey();
			key = Convert.ToBase64String(algorithm.Key);
		}

		/// <summary>
		/// Encrypt a string
		/// </summary>
		/// <param name="value">String to be encrypted</param>
		public string Encrypt(string value)
		{
			algorithm.Key = Convert.FromBase64String(key);
			algorithm.GenerateIV();

			var iv = algorithm.IV;

			byte[] output = new byte[value.Length * 4];
			byte[] input = Encoding.UTF8.GetBytes(value);

			value = Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
			ICryptoTransform encryptor = algorithm.CreateEncryptor();
			output = encryptor.TransformFinalBlock(input, 0, input.Length);

			var ivData = MergeIVAndData(iv, output);

			algorithm.Clear();

			return Convert.ToBase64String(ivData);
		}

		/// <summary>
		/// Decrypt a string
		/// </summary>
		/// <param name="value">String to be decrypted</param>
		public string Decrypt(string value)
		{
			algorithm.Key = Convert.FromBase64String(key);

			var ivData = Convert.FromBase64String(value);
			var iv = GetIV(ivData, algorithm.IV.Length);
			var data = GetData(ivData, algorithm.IV.Length);

			byte[] output = new byte[value.Length * 4];

			ICryptoTransform decryptor = algorithm.CreateDecryptor();
			output = decryptor.TransformFinalBlock(data, 0, data.Length);
			string resultado = Encoding.ASCII.GetString(output);

			algorithm.Clear();

			return resultado;
		}
		#endregion
	}
}
