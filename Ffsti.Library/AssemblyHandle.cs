using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ffsti
{
    public static class AssemblyHandle
    {
        public static Version RetrieveVersionFromAssembly(Assembly assembly)
        {
            return assembly.GetName().Version;
        }
    }
}
