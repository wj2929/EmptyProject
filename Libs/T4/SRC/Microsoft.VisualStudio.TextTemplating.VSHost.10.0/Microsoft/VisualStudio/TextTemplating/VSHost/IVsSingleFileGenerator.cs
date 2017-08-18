namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("3634494C-492F-4F91-8009-4541234E4E99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Obsolete("Please use Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator")]
    public interface IVsSingleFileGenerator
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDefaultExtension();
        void Generate([MarshalAs(UnmanagedType.LPWStr)] string inputFilePath, [MarshalAs(UnmanagedType.BStr)] string inputFileContents, [MarshalAs(UnmanagedType.LPWStr)] string defaultNamespace, out IntPtr outputFileContents, [MarshalAs(UnmanagedType.U4)] out int output, IVsGeneratorProgress generateProgress);
    }
}

