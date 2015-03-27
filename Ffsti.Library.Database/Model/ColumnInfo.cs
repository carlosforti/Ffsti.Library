using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database
{
	public class ColumnInfo
	{
		public string Name { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsNullable { get; set; }
		public bool IsAutoIncrement { get; set; }

		public override string ToString()
		{
			return this.Name;
		}

		public override bool Equals(object obj)
		{
			var comparisionObj = (obj as ColumnInfo);

			return comparisionObj.Name == this.Name &&
				comparisionObj.IsNullable == this.IsNullable &&
				comparisionObj.IsPrimaryKey == this.IsPrimaryKey &&
				comparisionObj.IsAutoIncrement == this.IsAutoIncrement;
		}
	}
}
