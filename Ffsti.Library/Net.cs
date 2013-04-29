using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Ffsti.Library
{
    public class Net
    {
        [DllImport("wininet.dll", SetLastError = true)]
        extern static bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        public static bool IsConnected()
        {
            int flags;
            return InternetGetConnectedState(out flags, 0);
        }
    }
}
