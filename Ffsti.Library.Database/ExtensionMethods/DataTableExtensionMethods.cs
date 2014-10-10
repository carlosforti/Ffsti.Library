using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ffsti.Library.Database
{
	/// <summary>
	/// Data Table Extension Methods
	/// </summary>
    public static class DataTableExtensionMethods
    {
		/// <summary>
		/// <para>Generate a List of Generics from a data table.</para>
		/// <para>The column name must be identical to the property name</para>
		/// </summary>
		/// <typeparam name="T">The type to be returned</typeparam>
		/// <param name="table">The data table to use</param>
		/// <returns>A List of T objects</returns>
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
                        obj.GetType().GetProperty(propertyName).SetValue(obj, row[propertyName], null);
                }

                result.Add(obj);
            }

            return result;
        }
    }
}