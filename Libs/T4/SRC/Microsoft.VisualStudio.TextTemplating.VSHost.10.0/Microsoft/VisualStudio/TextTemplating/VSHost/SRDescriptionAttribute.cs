namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        private bool isReplaced;

        public SRDescriptionAttribute(string descriptionResource) : base(descriptionResource)
        {
        }

        public override string Description
        {
            get
            {
                if (!this.isReplaced)
                {
                    this.isReplaced = true;
                    base.DescriptionValue = Resources.ResourceManager.GetString(base.DescriptionValue);
                }
                return base.DescriptionValue;
            }
        }
    }
}

