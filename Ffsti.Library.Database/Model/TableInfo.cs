using System.Linq;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Ffsti.Library.Database
{
	public class TableInfo
	{
		public string Name { get; set; }

		public List<ColumnInfo> Columns { get; set; }

		public List<ColumnInfo> AutoIncrementColumns
		{
			get { return Columns.Where(c => c.IsAutoIncrement).ToList(); }
		}

		public List<ColumnInfo> PrimaryKeys
		{
			get { return Columns.Where(c => c.IsPrimaryKey).ToList(); }
		}

		public List<ColumnInfo> NullableColumns
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
			bool ultimoItem = false;
			bool primeiroItem = true;

			StringBuilder query = new StringBuilder();
			query.AppendLine("SELECT ");

			foreach (var column in Columns)
			{
				ultimoItem = (this.Columns.Last().Equals(column));
				query.AppendFormat("       {0}{1}\n", column, (ultimoItem ? "" : ","));
			}

			ultimoItem = true;

			query.AppendLine("FROM");
			query.AppendFormat("       {0}\n", this.Name);

			if (createWhereWithPrimaryKeys)
			{
				query.AppendLine("WHERE");

				foreach (var column in Columns.Where(c => c.IsPrimaryKey))
				{
					query.AppendFormat("{1}    {0} = @{0}\n", column, (primeiroItem ? "   " : "AND"));
					ultimoItem = false ;
				}
			}

			return query.ToString();
		}

		public string CreateInsert()
		{
			bool ultimoItem = true;

			StringBuilder dml = new StringBuilder();
			StringBuilder param = new StringBuilder();

			dml.AppendFormat("INSERT INTO {0} (\n", this.Name);

			foreach (var column in this.Columns)
			{
				if (!column.IsAutoIncrement)
				{
					ultimoItem = (this.Columns.Last().Equals(column));
					dml.AppendFormat("  {0}{1}\n", column.Name, (ultimoItem ? ")" : ","));
					param.AppendFormat("  @{0}{1}\n", column.Name, (ultimoItem ? ")" : ","));
				}
			}

			dml.AppendLine("VALUES (");
			dml.AppendLine(param.ToString());

			return dml.ToString();
		}

		public string CreateUpdate()
		{
			bool ultimoItem = false;
			bool primeiroItem = true;

			StringBuilder dml = new StringBuilder();
			dml.AppendFormat("UPDATE {0}\n", this.Name);
			dml.AppendLine("SET");

			foreach (var column in Columns.Where(c => !c.IsAutoIncrement && !c.IsPrimaryKey))
			{
				ultimoItem = (this.Columns.Last().Equals(column));
				dml.AppendFormat("       {0} = @{0}{1}\n", column, (ultimoItem ? "" : ","));
			}

			dml.AppendLine("WHERE");

			foreach (var column in this.PrimaryKeys)
			{
				dml.AppendFormat("{1}    {0} = @{0}\n", column, (primeiroItem ? "   " : "AND"));
				primeiroItem = false;
			}

			return dml.ToString();
		}

		public string CreateDelete()
		{
			bool primeiroItem = true;
			StringBuilder dml = new StringBuilder();

			dml.AppendFormat("DELETE FROM {0}\n", this.Name);
			dml.AppendLine("WHERE");

			foreach(var column in PrimaryKeys)
			{
				dml.AppendFormat("{1}    {0} = @{0}\n", column, primeiroItem ? "   " : "AND");
				primeiroItem = false;
			}

			return dml.ToString();
		}
	}
}
