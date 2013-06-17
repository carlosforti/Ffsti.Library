using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database
{
    public static class CommandExtensionMethods
    {
        public static int AddParameter(this IDbCommand command, string parameterName, object value,
            DbType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            System.Data.IDataParameter param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbType;
            param.Direction = parameterDirection;

            return command.Parameters.Add(param);
        }
    }
}
