using System.Collections.Generic;
using System.Data;

namespace Ffsti.Library.Database
{
	public static class ConnectionExtensionMethods
	{
		public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
		}

		public static int Execute(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return cnn.Execute(sql, param, transaction, commandTimeout, commandType);
		}

		public static IDataReader ExecuteReader(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
		}

		public static T ExecuteScalar<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return cnn.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
		}
	}
}
