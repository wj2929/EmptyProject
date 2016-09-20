namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Diagnostics;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true), CLSCompliant(false)]
    public sealed class ProvideDirectiveProcessorAttribute : RegistrationAttribute
    {
        private string description;
        public const string DirectiveProcessorsKeyName = "DirectiveProcessors";
        private string name;
        public const string TextTemplatingKeyName = "TextTemplating";
        private System.Type type;

        public ProvideDirectiveProcessorAttribute(System.Type type, string name, string description)
        {
            this.type = type;
            this.name = name;
            this.description = description;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            using (RegistrationAttribute.Key key = context.CreateKey("TextTemplating"))
            {
                if (key != null)
                {
                    using (RegistrationAttribute.Key key2 = key.CreateSubkey("DirectiveProcessors"))
                    {
                        if (key2 != null)
                        {
                            using (RegistrationAttribute.Key key3 = key2.CreateSubkey(this.Name))
                            {
                                if (key3 != null)
                                {
                                    key3.SetValue("", this.Description);
                                    key3.SetValue("Class", this.Type.FullName);
                                    if (context.RegistrationMethod == RegistrationMethod.CodeBase)
                                    {
                                        key3.SetValue("CodeBase", this.Type.Assembly.Location);
                                    }
                                    else
                                    {
                                        key3.SetValue("Assembly", this.Type.Assembly.FullName);
                                    }
                                    context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideDirectiveProcessorLogRegistered, new object[] { this.Type.FullName, this.Name, this.Description }));
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.RemoveKey(@"TextTemplating\DirectiveProcessors\" + this.Name);
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideDirectiveProcessorLogUnregistered, new object[] { this.Type.FullName, this.Name, this.Description }));
        }

        public string Description
        {
            [DebuggerStepThrough]
            get
            {
                return this.description;
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

