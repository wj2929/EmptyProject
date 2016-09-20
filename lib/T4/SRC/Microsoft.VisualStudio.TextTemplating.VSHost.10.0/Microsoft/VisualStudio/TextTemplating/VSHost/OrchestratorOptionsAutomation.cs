namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;

    [CLSCompliant(false)]
    public class OrchestratorOptionsAutomation : IProfileManager
    {
        internal static OrchestratorOptionsAutomation instance;
        private IServiceProvider serviceProvider;
        private const string settingsRegistryPath = "DSLTools";
        private static bool showWarningDialogValue = true;

        public OrchestratorOptionsAutomation(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.LoadSettingsFromStorage();
            instance = this;
        }

        public void LoadSettingsFromStorage()
        {
            Package service = (Package) this.ServiceProvider.GetService(typeof(Package));
            if (service != null)
            {
                using (RegistryKey key = service.UserRegistryRoot)
                {
                    if (key != null)
                    {
                        string name = "DSLTools";
                        using (RegistryKey key2 = key.OpenSubKey(name, false))
                        {
                            if (key2 != null)
                            {
                                string[] valueNames = key2.GetValueNames();
                                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
                                for (int i = 0; i < valueNames.Length; i++)
                                {
                                    string str2 = valueNames[i];
                                    string text = key2.GetValue(str2).ToString();
                                    PropertyDescriptor descriptor = properties[str2];
                                    if ((descriptor != null) && descriptor.Converter.CanConvertFrom(typeof(string)))
                                    {
                                        descriptor.SetValue(this, descriptor.Converter.ConvertFromInvariantString(text));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void LoadSettingsFromXml(IVsSettingsReader reader)
        {
        }

        public void ResetSettings()
        {
            Package service = (Package) this.ServiceProvider.GetService(typeof(Package));
            if (service != null)
            {
                using (RegistryKey key = service.UserRegistryRoot)
                {
                    key.DeleteSubKey("DSLTools", false);
                }
            }
        }

        public void SaveSettingsToStorage()
        {
            Package service = (Package) this.ServiceProvider.GetService(typeof(Package));
            if (service != null)
            {
                using (RegistryKey key = service.UserRegistryRoot)
                {
                    string name = "DSLTools";
                    RegistryKey key2 = key.OpenSubKey(name, true);
                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey(name);
                    }
                    try
                    {
                        Attribute[] attributes = new Attribute[] { DesignerSerializationVisibilityAttribute.Visible };
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this, attributes))
                        {
                            TypeConverter converter = descriptor.Converter;
                            if (converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
                            {
                                key2.SetValue(descriptor.Name, converter.ConvertToInvariantString(descriptor.GetValue(this)));
                            }
                        }
                    }
                    finally
                    {
                        if (key2 != null)
                        {
                            key2.Dispose();
                        }
                    }
                }
            }
        }

        public void SaveSettingsToXml(IVsSettingsWriter writer)
        {
        }

        internal static bool ShowSecurityWarningDialog(IServiceProvider serviceProvider)
        {
            serviceProvider.GetService(typeof(STextTemplating));
            bool cancel = false;
            if (showWarningDialogValue)
            {
                SecurityWarningDialog.ShowSecurityWarningDialog(serviceProvider, out cancel, out showWarningDialogValue);
                instance.SaveSettingsToStorage();
            }
            return cancel;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }

        [Microsoft.VisualStudio.TextTemplating.VSHost.SRDescription("SecurityDialogOptionDescription"), DefaultValue(true), SRDisplayName("SecurityDialogOptionDisplayName"), Microsoft.VisualStudio.TextTemplating.VSHost.SRCategory("SecurityDialogOptionCategory"), TypeConverter(typeof(LocalizedBooleanTypeConverter))]
        public bool ShowWarningDialog
        {
            get
            {
                return showWarningDialogValue;
            }
            set
            {
                showWarningDialogValue = value;
            }
        }
    }
}

