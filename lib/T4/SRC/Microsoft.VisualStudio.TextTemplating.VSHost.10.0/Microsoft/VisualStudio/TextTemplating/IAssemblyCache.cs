namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("E707DCDE-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAssemblyCache
    {
        int UninstallAssembly();
        [PreserveSig]
        int QueryAssemblyInfo(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, ref ASSEMBLY_INFO pAsmInfo);
        int CreateAssemblyCacheItem();
        int CreateAssemblyScavenger();
        int InstallAssembly();
    }
}

