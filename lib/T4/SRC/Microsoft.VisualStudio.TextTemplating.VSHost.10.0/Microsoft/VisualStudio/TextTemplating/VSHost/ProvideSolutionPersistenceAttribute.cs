namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    internal sealed class ProvideSolutionPersistenceAttribute : RegistrationAttribute
    {
        private string name;
        private const string solutionPersistenceKeyName = "SolutionPersistence";

        public ProvideSolutionPersistenceAttribute(string name)
        {
            this.name = name;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            bool flag = false;
            RegistrationAttribute.Key key = context.CreateKey("SolutionPersistence");
            if (key != null)
            {
                RegistrationAttribute.Key key2 = key.CreateSubkey(this.Name);
                if (key2 != null)
                {
                    key2.SetValue("", "{" + context.ComponentType.GUID.ToString() + "}");
                    flag = true;
                }
            }
            if (flag)
            {
                context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideSolutionPersistenceLogRegistered, new object[] { this.Name }));
            }
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.RemoveKey(@"SolutionPersistence\" + this.Name);
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideSolutionPersistenceLogUnregistered, new object[] { this.Name }));
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}

