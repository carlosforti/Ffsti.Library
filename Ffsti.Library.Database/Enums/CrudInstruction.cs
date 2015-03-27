using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database.Enums
{
	public enum CrudInstruction: int
	{
		Create = 0,
		Query = 1,
		Update = 2,
		Delete = 3
	}
}
