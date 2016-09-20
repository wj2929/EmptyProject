namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Diagnostics;
    using System.Globalization;

    [CLSCompliant(false), AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public sealed class ProvideCodeGeneratorAttribute : RegistrationAttribute
    {
        public const string AspNetProjectGuid = "{39c9c826-8ef8-4079-8c95-428f5b1c323f}";
        public const string CSharpProjectGuid = "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}";
        private string description;
        private bool generatesDesignTimeSource = true;
        private string name;
        private string projectSystem = "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}";
        private bool registerCodeBase;
        private System.Type type;
        public const string VisualBasicProjectGuid = "{164b10b9-b200-11d0-8c61-00a0c91e29d5}";

        public ProvideCodeGeneratorAttribute(System.Type type, string name, string description, bool generatesDesignTimeSource)
        {
            this.type = type;
            this.name = name;
            this.description = description;
            this.generatesDesignTimeSource = generatesDesignTimeSource;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.Type == null)
            {
                throw new ArgumentNullException("Type");
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentNullException("Name");
            }
            if (string.IsNullOrEmpty(this.ProjectSystem))
            {
                throw new ArgumentNullException("ProjectSystem");
            }
            using (RegistrationAttribute.Key key = context.CreateKey("Generators"))
            {
                using (RegistrationAttribute.Key key2 = key.CreateSubkey(this.ProjectSystem))
                {
                    using (RegistrationAttribute.Key key3 = key2.CreateSubkey(this.Name))
                    {
                        key3.SetValue("", this.Description);
                        key3.SetValue("CLSID", "{" + this.Type.GUID + "}");
                        key3.SetValue("GeneratesDesignTimeSource", this.GeneratesDesignTimeSource ? 1 : 0);
                    }
                }
                using (RegistrationAttribute.Key key4 = context.CreateKey("CLSID"))
                {
                    using (RegistrationAttribute.Key key5 = key4.CreateSubkey("{" + this.Type.GUID + "}"))
                    {
                        key5.SetValue("", this.Description);
                        key5.SetValue("Class", this.Type.FullName);
                        key5.SetValue("InprocServer32", context.InprocServerPath);
                        key5.SetValue("ThreadingModel", "Both");
                        if (context.RegistrationMethod == RegistrationMethod.CodeBase)
                        {
                            key5.SetValue("CodeBase", this.Type.Assembly.CodeBase);
                        }
                        else
                        {
                            key5.SetValue("Assembly", this.Type.Assembly.FullName);
                        }
                    }
                }
            }
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideCodeGeneratorAttributeRegisterLog, new object[] { this.Name, this.Type.GUID.ToString() }));
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.Type == null)
            {
                throw new ArgumentNullException("Type");
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentNullException("Name");
            }
            if (string.IsNullOrEmpty(this.ProjectSystem))
            {
                throw new ArgumentNullException("ProjectSystem");
            }
            context.RemoveKey(@"Generators\" + this.ProjectSystem + @"\" + this.Name);
            context.RemoveKey(@"CLSID\{" + this.Type.GUID + "}");
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideCodeGeneratorAttributeUnregisterLog, new object[] { this.Name, this.Type.GUID.ToString() }));
        }

        public string Description
        {
            [DebuggerStepThrough]
            get
            {
                return this.description;
            }
        }

        public bool GeneratesDesignTimeSource
        {
            [DebuggerStepThrough]
            get
            {
                return this.generatesDesignTimeSource;
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

        public bool RegisterCodeBase
        {
            [DebuggerStepThrough]
            get
            {
                return this.registerCodeBase;
            }
            [DebuggerStepThrough]
            set
            {
                this.registerCodeBase = value;
            }
        }

        public System.Type Type
        {
            [DebuggerStepThrough]
            get
            {
                return this.type;
            }
        }
    }
}

