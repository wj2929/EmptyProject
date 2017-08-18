namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal static class AssemblyCache
    {
        internal static Dictionary<string, AssemblyRecord> assemblies = new Dictionary<string, AssemblyRecord>(0x23);
        internal static DateTime lastUse;

        internal static Assembly Find(string fullClassName)
        {
            lock (assemblies)
            {
                AssemblyRecord record;
                lastUse = DateTime.Now;
                if (assemblies.TryGetValue(fullClassName, out record))
                {
                    record.LastUse = DateTime.Now;
                    return record.Assembly;
                }
                return null;
            }
        }

        internal static void Insert(string fullClassName, Assembly assembly)
        {
            lock (assemblies)
            {
                lastUse = DateTime.Now;
                AssemblyRecord record = new AssemblyRecord(assembly);
                assemblies[fullClassName] = record;
            }
        }
    }
}

