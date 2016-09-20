namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Modeling;
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRCategoryAttribute : CategoryAttribute
    {
        private bool isReplaced;
        private string localized;

        public SRCategoryAttribute(string categoryResource) : base(categoryResource)
        {
        }

        protected override string GetLocalizedString(string value)
        {
            if (!this.isReplaced)
            {
                this.isReplaced = true;
                try
                {
                    this.localized = Resources.ResourceManager.GetString(base.Category);
                }
                catch (Exception exception)
                {
                    if (CriticalException.IsCriticalException(exception))
                    {
                        throw;
                    }
                    return null;
                }
            }
            return this.localized;
        }
    }
}

