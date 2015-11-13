using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.WinForms.Classes
{
	public class ListItem
	{
		public int Value { get; set; }
		public string Text { get; set; }

		public ListItem(int value, string text)
		{
			this.Value = value;
			this.Text = text;
		}

		public ListItem()
			: this(-1, string.Empty) { }

		public override string ToString()
		{
			return this.Text;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return ((ListItem)obj).GetHashCode() == this.GetHashCode();
		}
	}
}
