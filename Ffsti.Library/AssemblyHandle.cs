using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ffsti
{
    /// <summary>
    /// Class for handle Assemblies Values
    /// </summary>
    public static class AssemblyHandle
    {
        /// <summary>
        /// Retrieve the version for a given assembly
        /// </summary>
        /// <param name="assembly">Assembly to get version number</param>
        /// <returns>Version</returns>
        public static Version RetrieveVersionFromAssembly(Assembly assembly)
        {
            return assembly.GetName().Version;
        }
    }
}
