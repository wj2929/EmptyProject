namespace Microsoft.VisualStudio.Modeling
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class SafeWindowTarget : IWindowTarget
    {
        private IWindowTarget inner;
        private IServiceProvider serviceProvider;

        public SafeWindowTarget(IServiceProvider serviceProvider, IWindowTarget inner)
        {
            this.serviceProvider = serviceProvider;
            this.inner = inner;
        }

        internal static void ReplaceWindowTargetRecursive(IServiceProvider serviceProvider, ICollection controls)
        {
            ReplaceWindowTargetRecursive(serviceProvider, controls, true);
        }

        internal static void ReplaceWindowTargetRecursive(IServiceProvider serviceProvider, ICollection controls, bool checkControlAdded)
        {
            foreach (Control control in controls)
            {
                control.WindowTarget = new SafeWindowTarget(serviceProvider, control.WindowTarget);
                if (control.Controls.Count > 0)
                {
                    ReplaceWindowTargetRecursive(serviceProvider, control.Controls, checkControlAdded);
                }
            }
        }

        void IWindowTarget.OnHandleChange(IntPtr newHandle)
        {
            this.inner.OnHandleChange(newHandle);
        }

        void IWindowTarget.OnMessage(ref Message m)
        {
            try
            {
                this.inner.OnMessage(ref m);
            }
            catch (Exception exception)
            {
                if (CriticalException.ThrowOrShow(this.serviceProvider, exception))
                {
                    throw;
                }
            }
        }
    }
}

