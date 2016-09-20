namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        public const int E_INSUFFICIENT_BUFFER = -2147024774;
        public const uint VSITEMID_ROOT = 0xfffffffe;

        [DllImport("fusion.dll", CharSet=CharSet.Auto)]
        internal static extern int CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved);
        public static bool Failed(int hr)
        {
            return (hr < 0);
        }
    }
}

