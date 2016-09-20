namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("BED89B98-6EC9-43CB-B0A8-41D6E2D6669D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Obsolete("Please use Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress")]
    public interface IVsGeneratorProgress
    {
        void GeneratorError([MarshalAs(UnmanagedType.Bool)] bool warning, [MarshalAs(UnmanagedType.U4)] int level, [MarshalAs(UnmanagedType.BStr)] string errorText, [MarshalAs(UnmanagedType.U4)] int line, [MarshalAs(UnmanagedType.U4)] int column);
        void Progress([MarshalAs(UnmanagedType.U4)] int complete, [MarshalAs(UnmanagedType.U4)] int total);
    }
}

