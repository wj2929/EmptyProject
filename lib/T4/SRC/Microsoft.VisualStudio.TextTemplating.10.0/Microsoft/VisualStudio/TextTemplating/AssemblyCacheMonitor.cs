namespace Microsoft.VisualStudio.TextTemplating
{
    using System;

    public sealed class AssemblyCacheMonitor : MarshalByRefObject
    {
        public int GetStaleAssembliesCount(TimeSpan assemblyStaleTime)
        {
            int num = 0;
            lock (AssemblyCache.assemblies)
            {
                foreach (string str in AssemblyCache.assemblies.Keys)
                {
                    if ((AssemblyCache.lastUse - AssemblyCache.assemblies[str].LastUse) > assemblyStaleTime)
                    {
                        num++;
                    }
                }
            }
            return num;
        }
    }
}

