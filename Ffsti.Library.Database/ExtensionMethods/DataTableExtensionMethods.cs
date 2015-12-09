using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            var result = new List<T>();

            var columns = table.Columns;

            foreach (DataRow row in table.Rows)
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var propertyName in from DataColumn column in columns
                                             select column.ColumnName)
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, row[propertyName], null);
                }

                result.Add(obj);
            }

            return result;
        }
    }
}