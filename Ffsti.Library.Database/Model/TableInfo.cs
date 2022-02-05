using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TableInfo
    {
        private readonly List<ColumnInfo> _columns;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        public TableInfo(string name, IEnumerable<ColumnInfo> columns)
        {
            Name = name;
            _columns = columns.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<ColumnInfo> Columns => _columns;

        /// <summary>
        /// 
        /// </summary>
        public List<ColumnInfo> AutoIncrementColumns()
        {
            return Columns.Where(c => c.IsAutoIncrement).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ColumnInfo> PrimaryKeys()
        {
            return Columns.Where(c => c.IsPrimaryKey).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnInfo> NullableColumns()
        {
            return Columns.Where(c => c.IsNullable).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createWhereWithPrimaryKeys"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

            foreach (var column in PrimaryKeys())
            {
                dml.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
                primeiroItem = false;
            }

            return dml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateDelete()
        {
            var primeiroItem = true;
            var dml = new StringBuilder();

            dml.AppendFormat("DELETE FROM {0}\n", Name);
            dml.AppendLine("WHERE");

            foreach (var column in PrimaryKeys())
            {
                dml.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
                primeiroItem = false;
            }

            return dml.ToString();
        }
    }
}
