namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRDisplayNameAttribute : DisplayNameAttribute
    {
        private bool isReplaced;

        public SRDisplayNameAttribute(string displayNameResource) : base(displayNameResource)
        {
        }

        public override string DisplayName
        {
            get
            {
                if (!this.isReplaced)
                {
                    this.isReplaced = true;
                    base.DisplayNameValue = Resources.ResourceManager.GetString(base.DisplayNameValue);
                }
                return base.DisplayNameValue;
            }
        }
    }
}

