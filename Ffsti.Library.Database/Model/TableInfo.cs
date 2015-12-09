using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database.Model
{
    public class TableInfo
    {
        public string Name { get; set; }

        public List<ColumnInfo> Columns { get; set; }

        public List<ColumnInfo> AutoIncrementColumns
        {
            get { return Columns.Where(c => c.IsAutoIncrement).ToList(); }
        }

        public IEnumerable<ColumnInfo> PrimaryKeys
        {
            get { return Columns.Where(c => c.IsPrimaryKey).ToList(); }
        }

        public IEnumerable<ColumnInfo> NullableColumns
        {
            get { return Columns.Where(c => c.IsNullable).ToList(); }
        }

        //public bool IsPrimaryKey(string columnName)
        //{
        //	var column = Columns.FirstOrDefault(c => c.Name == columnName);
        //	if (column != null)
        //		return column.IsPrimaryKey;

        //	return false;
        //}

        //public bool IsNullable(string columnName)
        //{
        //	var column = Columns.FirstOrDefault(c => c.Name == columnName);
        //	if (column != null)
        //		return column.IsNullable;

        //	return false;
        //}

        //public bool IsAutoIncrement(string columnName)
        //{
        //	var column = Columns.FirstOrDefault(c => c.Name == columnName);
        //	if (column != null)
        //		return column.IsAutoIncrement;

        //	return false;
        //}

        public string CreateQuery(bool createWhereWithPrimaryKeys)
        {
            const bool primeiroItem = true;

            var query = new StringBuilder();
            query.AppendLine("SELECT ");

            foreach (var column in Columns)
            {
                var ultimoItem = Columns.Last().Equals(column);
                query.AppendFormat("       {0}{1}\n", column, ultimoItem ? "" : ",");
            }

            query.AppendLine("FROM");
            query.AppendFormat("       {0}\n", Name);

            if (!createWhereWithPrimaryKeys)
                return query.ToString();

            query.AppendLine("WHERE");

            foreach (var column in Columns.Where(c => c.IsPrimaryKey))
            {
                query.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
            }

            return query.ToString();
        }

        public string CreateInsert()
        {
            var dml = new StringBuilder();
            var param = new StringBuilder();

            dml.AppendFormat("INSERT INTO {0} (\n", Name);

            foreach (var column in Columns)
            {
                if (column.IsAutoIncrement)
                    continue;

                var ultimoItem = Columns.Last().Equals(column);
                dml.AppendFormat("  {0}{1}\n", column.Name, ultimoItem ? ")" : ",");
                param.AppendFormat("  @{0}{1}\n", column.Name, ultimoItem ? ")" : ",");
            }

            dml.AppendLine("VALUES (");
            dml.AppendLine(param.ToString());

            return dml.ToString();
        }

        public string CreateUpdate()
        {
            var primeiroItem = true;

            var dml = new StringBuilder();
            dml.AppendFormat("UPDATE {0}\n", Name);
            dml.AppendLine("SET");

            foreach (var column in Columns.Where(c => !c.IsAutoIncrement && !c.IsPrimaryKey))
            {
                var ultimoItem = Columns.Last().Equals(column);
                dml.AppendFormat("       {0} = @{0}{1}\n", column, ultimoItem ? "" : ",");
            }

            dml.AppendLine("WHERE");

            foreach (var column in PrimaryKeys)
            {
                dml.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
                primeiroItem = false;
            }

            return dml.ToString();
        }

        public string CreateDelete()
        {
            var primeiroItem = true;
            var dml = new StringBuilder();

            dml.AppendFormat("DELETE FROM {0}\n", Name);
            dml.AppendLine("WHERE");

            foreach (var column in PrimaryKeys)
            {
                dml.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
                primeiroItem = false;
            }

            return dml.ToString();
        }
    }
}
