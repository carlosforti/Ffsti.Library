using System.Data;

namespace Ffsti.Library.Database
{
	/// <summary>
	/// Extension Methods for IDbCommand
	/// </summary>
	public static class CommandExtensionMethods
	{
		/// <summary>
		/// Add a parameter to the IDbCommand
		/// </summary>
		/// <param name="command">The IDbCommand to add a parameter</param>
		/// <param name="parameterName">Parameter name</param>
		/// <param name="value">Parameter Value</param>
		/// <param name="dbType">Parameter data type</param>
		/// <param name="parameterDirection">Parameter direction</param>
		/// <returns></returns>
		public static int AddParameter(this IDbCommand command, string parameterName, object value,
			DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
		{
			IDataParameter param = command.CreateParameter();

			param.ParameterName = parameterName;
			param.Value = value;
			param.DbType = dbType;
			param.Direction = parameterDirection;

			return command.Parameters.Add(param);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int AddParameter(this IDbCommand command, string parameterName, object value)
		{
			IDataParameter param = command.CreateParameter();

			param.ParameterName = parameterName;
			param.Value = value;

			return command.Parameters.Add(param);
		}
	}
}
