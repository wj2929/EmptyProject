namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Reflection;

    [Serializable]
    internal class AssemblyResolver
    {
        public Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (StringComparer.OrdinalIgnoreCase.Compare(assembly.FullName, args.Name) == 0)
                {
                    return assembly;
                }
            }
            return null;
        }
    }
}

