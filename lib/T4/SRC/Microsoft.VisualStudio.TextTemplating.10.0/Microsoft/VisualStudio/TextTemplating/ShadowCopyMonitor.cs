namespace Microsoft.VisualStudio.TextTemplating
{
    using System;

    public sealed class ShadowCopyMonitor : MarshalByRefObject
    {
        public bool AreShadowCopiesObsolete
        {
            get
            {
                return ShadowTimes.AreShadowCopiesObsolete;
            }
        }
    }
}

