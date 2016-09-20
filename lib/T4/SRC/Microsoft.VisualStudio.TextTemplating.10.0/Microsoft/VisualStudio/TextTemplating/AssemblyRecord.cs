namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Reflection;

    internal class AssemblyRecord
    {
        private System.Reflection.Assembly assembly;
        private DateTime lastUse;

        internal AssemblyRecord(System.Reflection.Assembly assembly)
        {
            this.assembly = assembly;
            this.lastUse = DateTime.Now;
        }

        internal System.Reflection.Assembly Assembly
        {
            get
            {
                return this.assembly;
            }
        }

        internal DateTime LastUse
        {
            get
            {
                return this.lastUse;
            }
            set
            {
                this.lastUse = value;
            }
        }
    }
}

