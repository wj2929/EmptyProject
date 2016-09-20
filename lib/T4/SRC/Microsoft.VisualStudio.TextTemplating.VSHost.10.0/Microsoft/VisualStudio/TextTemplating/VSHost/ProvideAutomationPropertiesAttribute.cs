namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    internal sealed class ProvideAutomationPropertiesAttribute : RegistrationAttribute
    {
        private const string automationPropertiesKeyName = "AutomationProperties";
        private string category;
        private string subcategory;

        public ProvideAutomationPropertiesAttribute(string category, string subcategory)
        {
            this.category = category;
            this.subcategory = subcategory;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            bool flag = false;
            RegistrationAttribute.Key key = context.CreateKey("AutomationProperties");
            if (key != null)
            {
                RegistrationAttribute.Key key2 = key.CreateSubkey(this.Category);
                if (key2 != null)
                {
                    RegistrationAttribute.Key key3 = key2.CreateSubkey(this.Subcategory);
                    if (key3 != null)
                    {
                        key3.SetValue("Name", this.Category + "." + this.Subcategory);
                        key3.SetValue("Package", "{" + context.ComponentType.GUID.ToString() + "}");
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideAutomationPropertiesLogRegistered, new object[] { this.Category, this.Subcategory }));
            }
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.RemoveKey(@"AutomationProperties\" + this.Category + @"\" + this.Subcategory);
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideAutomationPropertiesLogUnregistered, new object[] { this.Category, this.Subcategory }));
        }

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        public string Subcategory
        {
            get
            {
                return this.subcategory;
            }
            set
            {
                this.subcategory = value;
            }
        }
    }
}

