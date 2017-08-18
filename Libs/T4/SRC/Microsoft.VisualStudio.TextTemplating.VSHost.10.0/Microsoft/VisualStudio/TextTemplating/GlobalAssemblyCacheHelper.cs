namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal static class GlobalAssemblyCacheHelper
    {
        internal static string GetLocation(string strongName)
        {
            IAssemblyCache cache;
            if (string.IsNullOrEmpty(strongName))
            {
                throw new ArgumentNullException("strongName");
            }
            AssemblyName name = new AssemblyName(strongName);
            string str = null;
            if (!Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(Microsoft.VisualStudio.TextTemplating.NativeMethods.CreateAssemblyCache(out cache, 0)) && (cache != null))
            {
                try
                {
                    if (name.ProcessorArchitecture == ProcessorArchitecture.None)
                    {
                        str = GetLocationImpl(cache, strongName, "x86");
                        if ((str != null) && (str.Length > 0))
                        {
                            return str;
                        }
                        str = GetLocationImpl(cache, strongName, "MSIL");
                        if ((str != null) && (str.Length > 0))
                        {
                            return str;
                        }
                        return GetLocationImpl(cache, strongName, null);
                    }
                    str = GetLocationImpl(cache, strongName, null);
                }
                finally
                {
                    Marshal.FinalReleaseComObject(cache);
                }
            }
            return str;
        }

        private static string GetLocationImpl(IAssemblyCache assemblyCache, string strongName, string targetProcessorArchitecture)
        {
            ASSEMBLY_INFO pAsmInfo = new ASSEMBLY_INFO {
                cbAssemblyInfo = (uint) Marshal.SizeOf(typeof(ASSEMBLY_INFO))
            };
            if (targetProcessorArchitecture != null)
            {
                strongName = strongName + ", ProcessorArchitecture=" + targetProcessorArchitecture;
            }
            int hr = assemblyCache.QueryAssemblyInfo(3, strongName, ref pAsmInfo);
            if ((Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(hr) && (hr != -2147024774)) || (pAsmInfo.cbAssemblyInfo == 0))
            {
                return string.Empty;
            }
            pAsmInfo.pszCurrentAssemblyPathBuf = new string(new char[pAsmInfo.cchBuf]);
            if (Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(assemblyCache.QueryAssemblyInfo(3, strongName, ref pAsmInfo)))
            {
                return string.Empty;
            }
            return pAsmInfo.pszCurrentAssemblyPathBuf;
        }
    }
}

