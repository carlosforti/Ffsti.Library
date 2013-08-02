using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ffsti.Library.Database
{
    public static class DataTableExtensionMethods
    {
        public static List<T> ToGenericList<T>(this DataTable table)
            where T : class, new()
        {
            T obj = Activator.CreateInstance<T>();
            List<T> result = new List<T>();

            var columns = table.Columns;

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in columns)
                {
                    string propertyName = column.ColumnName;
                    var property = obj.GetType().GetProperties().Where(p => p.Name == propertyName);

                    if (property != null)
                        obj.SetPropertyValue(propertyName, row[propertyName]);
                }

                result.Add(obj);
            }

            return result;
        }
    }
}