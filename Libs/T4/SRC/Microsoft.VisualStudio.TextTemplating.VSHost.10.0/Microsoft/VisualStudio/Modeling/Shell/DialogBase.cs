namespace Microsoft.VisualStudio.Modeling.Shell
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.VSHelp;
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal abstract class DialogBase : Form
    {
        private bool flagEnabledModeless;
        private const int HX_E_NOTFOUND = -2147220731;
        private IServiceProvider serviceProvider;
        private bool sizable;

        protected DialogBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.AccessibleRole = AccessibleRole.Dialog;
            base.StartPosition = FormStartPosition.CenterParent;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
        }

        protected override void Dispose(bool disposing)
        {
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
            base.Dispose(disposing);
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (this.ShowHelp())
            {
                hevent.Handled = true;
            }
            else
            {
                base.OnHelpRequested(hevent);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.SetFonts();
            base.OnLoad(e);
        }

        protected virtual void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            this.SetFonts();
        }

        [UIPermission(SecurityAction.LinkDemand, Window=UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
            bool flag = false;
            try
            {
                flag = base.ProcessDialogChar(charCode);
            }
            catch (Exception exception)
            {
                if (CriticalException.ThrowOrShow(this.serviceProvider, exception))
                {
                    throw;
                }
            }
            return flag;
        }

        [UIPermission(SecurityAction.LinkDemand, Window=UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool flag = false;
            try
            {
                flag = base.ProcessDialogKey(keyData);
            }
            catch (Exception exception)
            {
                if (CriticalException.ThrowOrShow(this.serviceProvider, exception))
                {
                    throw;
                }
            }
            return flag;
        }

        private void SetFonts()
        {
            this.Font = this.DialogFont;
        }

        private bool ShowHelp()
        {
            string str = this.F1Keyword;
            if (!string.IsNullOrEmpty(str) && (this.serviceProvider != null))
            {
                Microsoft.VisualStudio.VSHelp.Help service = this.serviceProvider.GetService(typeof(Microsoft.VisualStudio.VSHelp.Help)) as Microsoft.VisualStudio.VSHelp.Help;
                if (service != null)
                {
                    try
                    {
                        service.DisplayTopicFromF1Keyword(str);
                        return true;
                    }
                    catch (COMException exception)
                    {
                        if (exception.ErrorCode != -2147220731)
                        {
                            throw;
                        }
                    }
                }
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 0x112)
                {
                    if (((int) m.WParam) == 0xf180)
                    {
                        this.ShowHelp();
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception exception)
            {
                if (CriticalException.ThrowOrShow(this.serviceProvider, exception))
                {
                    throw;
                }
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags=SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style |= -2147483648;
                if (this.sizable)
                {
                    createParams.Style |= 0x40000;
                }
                if (this.serviceProvider != null)
                {
                    IVsUIShell service = this.serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
                    IntPtr zero = IntPtr.Zero;
                    if ((service == null) || !ErrorHandler.Succeeded(service.GetDialogOwnerHwnd(out zero)))
                    {
                        return createParams;
                    }
                    if (this.EnableModeless && !this.flagEnabledModeless)
                    {
                        int fEnable = 1;
                        if (ErrorHandler.Succeeded(service.EnableModeless(fEnable)))
                        {
                            this.flagEnabledModeless = true;
                        }
                    }
                    createParams.Parent = zero;
                }
                return createParams;
            }
        }

        protected Font DialogFont
        {
            get
            {
                IUIService service = this.ServiceProvider.GetService(typeof(IUIService)) as IUIService;
                if (service != null)
                {
                    Font font = service.Styles["DialogFont"] as Font;
                    if (font != null)
                    {
                        return font;
                    }
                }
                return this.Font;
            }
        }

        protected virtual bool DisableSafeWindowTargetHardeningCheck
        {
            get
            {
                return false;
            }
        }

        protected virtual bool EnableModeless
        {
            get
            {
                return false;
            }
        }

        protected virtual string F1Keyword
        {
            get
            {
                return string.Empty;
            }
        }

        public System.Windows.Forms.FormBorderStyle FormBorderStyle
        {
            get
            {
                if (this.sizable)
                {
                    return System.Windows.Forms.FormBorderStyle.Sizable;
                }
                return base.FormBorderStyle;
            }
            set
            {
                if (value == System.Windows.Forms.FormBorderStyle.Sizable)
                {
                    this.sizable = true;
                    base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                }
                else
                {
                    this.sizable = false;
                    base.FormBorderStyle = value;
                }
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }
    }
}

