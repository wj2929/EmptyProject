namespace Microsoft.VisualStudio.TextTemplating
{
    using System;

    public interface IRecognizeHostSpecific
    {
        void SetProcessingRunIsHostSpecific(bool hostSpecific);

        bool RequiresProcessingRunIsHostSpecific { get; }
    }
}

