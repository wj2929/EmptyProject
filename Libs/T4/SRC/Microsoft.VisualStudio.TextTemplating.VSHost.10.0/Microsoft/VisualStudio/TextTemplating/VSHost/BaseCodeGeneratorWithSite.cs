﻿namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextTemplating;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public abstract class BaseCodeGeneratorWithSite : BaseCodeGenerator, IObjectWithSite
    {
        private ServiceProvider globalProvider;
        private ServiceProvider serviceProvider;
        private object site;

        protected BaseCodeGeneratorWithSite()
        {
        }

        protected virtual string CreateExceptionMessage(Exception exception)
        {
            StringBuilder builder = new StringBuilder((exception.Message != null) ? exception.Message : string.Empty);
            for (Exception exception2 = exception.InnerException; exception2 != null; exception2 = exception2.InnerException)
            {
                string message = exception2.Message;
                if ((message != null) && (message.Length > 0))
                {
                    builder.AppendLine(" " + message);
                }
            }
            return builder.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (this.serviceProvider != null)
                {
                    this.serviceProvider.Dispose();
                    this.serviceProvider = null;
                }
                if (this.globalProvider != null)
                {
                    this.globalProvider.Dispose();
                    this.globalProvider = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected object GetService(Guid service)
        {
            return this.SiteServiceProvider.GetService(service);
        }

        protected object GetService(Type service)
        {
            return this.SiteServiceProvider.GetService(service);
        }

        public virtual void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (this.site == null)
            {
                Marshal.ThrowExceptionForHR(-2147467259);
            }
            IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(this.site);
            IntPtr zero = IntPtr.Zero;
            Marshal.QueryInterface(iUnknownForObject, ref riid, out zero);
            if (zero == IntPtr.Zero)
            {
                Marshal.ThrowExceptionForHR(-2147467262);
            }
            ppvSite = zero;
        }

        public virtual void SetSite(object pUnkSite)
        {
            this.site = pUnkSite;
            this.serviceProvider = null;
        }

        protected void SetWaitCursor()
        {
            if (this.site != null)
            {
                IVsHierarchy service = this.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                if (service != null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP = null;
                    if (service.GetSite(out ppSP) == 0)
                    {
                        IntPtr ptr;
                        Guid gUID = typeof(SVsUIShell).GUID;
                        Guid riid = typeof(IVsUIShell).GUID;
                        if (ErrorHandler.Succeeded(ppSP.QueryService(ref gUID, ref riid, out ptr)))
                        {
                            IVsUIShell objectForIUnknown = Marshal.GetObjectForIUnknown(ptr) as IVsUIShell;
                            if (objectForIUnknown != null)
                            {
                                objectForIUnknown.SetWaitCursor();
                            }
                            Marshal.Release(ptr);
                        }
                    }
                }
            }
        }

        [CLSCompliant(false)]
        protected DTE Dte
        {
            get
            {
                DTE objectForIUnknown = null;
                IVsHierarchy service = this.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                if (service != null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP = null;
                    if (!Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(service.GetSite(out ppSP)) && (ppSP != null))
                    {
                        Guid gUID = typeof(DTE).GUID;
                        IntPtr zero = IntPtr.Zero;
                        ErrorHandler.ThrowOnFailure(ppSP.QueryService(ref gUID, ref gUID, out zero));
                        if (zero != IntPtr.Zero)
                        {
                            objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as DTE;
                            Marshal.Release(zero);
                        }
                    }
                }
                return objectForIUnknown;
            }
        }

        [CLSCompliant(false)]
        protected IVsErrorList ErrorList
        {
            get
            {
                IVsErrorList objectForIUnknown = null;
                IVsHierarchy service = this.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                if (service != null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP = null;
                    if (!Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(service.GetSite(out ppSP)) && (ppSP != null))
                    {
                        Guid gUID = typeof(SVsErrorList).GUID;
                        Guid riid = typeof(IVsErrorList).GUID;
                        IntPtr zero = IntPtr.Zero;
                        ErrorHandler.ThrowOnFailure(ppSP.QueryService(ref gUID, ref riid, out zero));
                        if (zero != IntPtr.Zero)
                        {
                            objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as IVsErrorList;
                            Marshal.Release(zero);
                        }
                    }
                }
                return objectForIUnknown;
            }
        }

        [CLSCompliant(false)]
        protected ServiceProvider GlobalServiceProvider
        {
            get
            {
                if (this.globalProvider == null)
                {
                    ServiceProvider siteServiceProvider = this.SiteServiceProvider;
                    if (siteServiceProvider != null)
                    {
                        IVsHierarchy service = siteServiceProvider.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                        if (service != null)
                        {
                            Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP = null;
                            ErrorHandler.ThrowOnFailure(service.GetSite(out ppSP));
                            if (ppSP != null)
                            {
                                this.globalProvider = new ServiceProvider(ppSP);
                            }
                        }
                    }
                }
                return this.globalProvider;
            }
        }

        [CLSCompliant(false)]
        protected ServiceProvider SiteServiceProvider
        {
            get
            {
                if (this.serviceProvider == null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider site = this.site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    this.serviceProvider = new ServiceProvider(site);
                }
                return this.serviceProvider;
            }
        }
    }
}

