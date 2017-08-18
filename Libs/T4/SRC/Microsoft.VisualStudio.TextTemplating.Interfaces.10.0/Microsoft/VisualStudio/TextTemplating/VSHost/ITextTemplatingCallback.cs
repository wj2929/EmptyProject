namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.Text;

    [CLSCompliant(true)]
    public interface ITextTemplatingCallback
    {
        void ErrorCallback(bool warning, string message, int line, int column);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);
    }
}

