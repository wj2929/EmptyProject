namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.TextTemplating;
    using System;

    [CLSCompliant(false)]
    public interface ITextTemplatingComponents
    {
        ITextTemplatingCallback Callback { get; set; }

        ITextTemplatingEngine Engine { get; }

        object Hierarchy { get; set; }

        ITextTemplatingEngineHost Host { get; }

        string InputFile { get; set; }
    }
}

