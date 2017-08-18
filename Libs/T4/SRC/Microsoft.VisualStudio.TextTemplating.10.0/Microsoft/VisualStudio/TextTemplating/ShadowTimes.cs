namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;

    internal static class ShadowTimes
    {
        private static readonly ConcurrentDictionary<string, DateTime> times = new ConcurrentDictionary<string, DateTime>(1, 10, StringComparer.OrdinalIgnoreCase);

        internal static void Insert(string assemblyLocation, DateTime time)
        {
            times.TryAdd(assemblyLocation, time);
        }

        private static bool IsAssemblyObsolete(string assemblyLocation, DateTime lastWriteTime)
        {
            DateTime time;
            return (times.TryGetValue(assemblyLocation, out time) && (time < lastWriteTime));
        }

        internal static bool AreShadowCopiesObsolete
        {
            get
            {
                if (AppDomain.CurrentDomain.ShadowCopyFiles)
                {
                    foreach (string str in times.Keys.ToList<string>())
                    {
                        if (File.Exists(str))
                        {
                            FileInfo info = new FileInfo(str);
                            if (IsAssemblyObsolete(str, info.LastWriteTime))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}

