namespace Microsoft.VisualStudio.Modeling
{
    using Microsoft.Win32;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal static class CriticalException
    {
        private static bool disableFiltering;
        private static volatile bool initialized;
        private const string RegistryKeyPath = @"Software\Microsoft\ModelingTools";
        private const string RegistryValueName = "DisableExceptionFilter";
        private static readonly object syncRoot = new object();

        internal static bool IsCriticalException(Exception ex)
        {
            return (DisableExceptionFilter || ((((ex is NullReferenceException) || (ex is StackOverflowException)) || ((ex is OutOfMemoryException) || (ex is ThreadAbortException))) || ((ex.InnerException != null) && IsCriticalException(ex.InnerException))));
        }

        internal static bool ThrowOrShow(IServiceProvider serviceProvider, Exception ex)
        {
            if (IsCriticalException(ex))
            {
                return true;
            }
            if (serviceProvider != null)
            {
                IUIService service = serviceProvider.GetService(typeof(IUIService)) as IUIService;
                if (service != null)
                {
                    service.ShowError(ex);
                    return false;
                }
            }
            MessageBoxOptions options = 0;
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                options = MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
            }
            MessageBox.Show(null, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, options);
            return false;
        }

        internal static bool DisableExceptionFilter
        {
            get
            {
                if (!initialized)
                {
                    lock (syncRoot)
                    {
                        if (!initialized)
                        {
                            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\ModelingTools", false))
                            {
                                object obj2 = (key != null) ? key.GetValue("DisableExceptionFilter") : null;
                                if ((obj2 != null) && (obj2.ToString() != "0"))
                                {
                                    disableFiltering = true;
                                }
                            }
                            initialized = true;
                        }
                    }
                }
                return disableFiltering;
            }
        }
    }
}

