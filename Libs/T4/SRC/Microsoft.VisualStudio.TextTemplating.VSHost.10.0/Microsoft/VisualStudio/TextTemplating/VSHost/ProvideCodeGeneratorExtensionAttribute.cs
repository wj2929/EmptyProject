namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Diagnostics;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false), CLSCompliant(false)]
    public sealed class ProvideCodeGeneratorExtensionAttribute : RegistrationAttribute
    {
        public const string AspNetProjectSystemGuid = "{E24C65DC-7377-472b-9ABA-BC803B73C61A}";
        public const string CSharpProjectSystemGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        private string extension;
        private string name;
        private string projectSystem = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        private string projectSystemPackage = "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}";
        public const string VisualBasicProjectSystemGuid = "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";

        public ProvideCodeGeneratorExtensionAttribute(string name, string extension)
        {
            this.name = name;
            this.extension = extension;
            if (!this.extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                this.extension = "." + this.extension;
            }
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentNullException("Name");
            }
            if (string.IsNullOrEmpty(this.Extension))
            {
                throw new ArgumentNullException("Extension");
            }
            if (string.IsNullOrEmpty(this.ProjectSystemPackage))
            {
                throw new ArgumentNullException("ProjectSystemPackage");
            }
            if (string.IsNullOrEmpty(this.ProjectSystem))
            {
                throw new ArgumentNullException("ProjectSystem");
            }
            using (RegistrationAttribute.Key key = context.CreateKey("Generators"))
            {
                using (RegistrationAttribute.Key key2 = key.CreateSubkey(this.ProjectSystemPackage))
                {
                    using (RegistrationAttribute.Key key3 = key2.CreateSubkey(this.Extension))
                    {
                        key3.SetValue(string.Empty, this.Name);
                    }
                }
            }
            if (StringComparer.OrdinalIgnoreCase.Compare(this.ProjectSystem, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
            {
                using (RegistrationAttribute.Key key4 = context.CreateKey("Projects"))
                {
                    using (RegistrationAttribute.Key key5 = key4.CreateSubkey(this.ProjectSystem))
                    {
                        using (RegistrationAttribute.Key key6 = key5.CreateSubkey("FileExtensions"))
                        {
                            using (RegistrationAttribute.Key key7 = key6.CreateSubkey(this.Extension))
                            {
                                key7.SetValue("CustomTool", this.Name);
                            }
                        }
                    }
                }
            }
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideCodeGeneratorExtensionAttributeRegisterLog, new object[] { this.Name, this.Extension }));
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentNullException("Name");
            }
            if (string.IsNullOrEmpty(this.Extension))
            {
                throw new ArgumentNullException("Extension");
            }
            if (string.IsNullOrEmpty(this.ProjectSystemPackage))
            {
                throw new ArgumentNullException("ProjectSystemPackage");
            }
            if (string.IsNullOrEmpty(this.ProjectSystem))
            {
                throw new ArgumentNullException("ProjectSystem");
            }
            context.RemoveKey(@"Generators\" + this.ProjectSystem + @"\" + this.Extension);
            if (StringComparer.OrdinalIgnoreCase.Compare(this.ProjectSystem, "{E24C65DC-7377-472b-9ABA-BC803B73C61A}") == 0)
            {
                context.RemoveKey(string.Format(CultureInfo.InvariantCulture, @"Projects\\\{{0}\}\\FileExtensions\\{1}", new object[] { this.ProjectSystem, this.Extension }));
            }
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideCodeGeneratorExtensionAttributeUnregisterLog, new object[] { this.Name, this.Extension }));
        }

        public string Extension
        {
            [DebuggerStepThrough]
            get
            {
                return this.extension;
            }
        }

        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return this.name;
            }
        }

        public string ProjectSystem
        {
            [DebuggerStepThrough]
            get
            {
                return this.projectSystem;
            }
            [DebuggerStepThrough]
            set
            {
                this.projectSystem = value;
            }
        }

        public string ProjectSystemPackage
        {
            [DebuggerStepThrough]
            get
            {
                return this.projectSystemPackage;
            }
            [DebuggerStepThrough]
            set
            {
                this.projectSystemPackage = value;
            }
        }
    }
}

