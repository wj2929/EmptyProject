namespace Microsoft.VisualStudio.TextTemplating
{
    using System;

    [CLSCompliant(true)]
    public interface ITextTemplatingSessionHost
    {
        ITextTemplatingSession CreateSession();

        ITextTemplatingSession Session { get; set; }
    }
}

