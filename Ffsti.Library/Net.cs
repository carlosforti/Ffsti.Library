using System.Security;
using System.Runtime.InteropServices;

namespace Ffsti.Library
{
    /// <summary>
    /// Methods to handle not and internet
    /// </summary>
    public static class Net
    {
        /// <summary>
        /// Verifies internet connection
        /// </summary>
        public static bool IsConnected()
        {
            int flags;
            
            return SafeNativeMethods.InternetGetConnectedState(out flags, 0);
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("wininet.dll", SetLastError = true)]
        internal static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);
    }
}
