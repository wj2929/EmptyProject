namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.Runtime.InteropServices;

    [CLSCompliant(false)]
    public interface ITextTemplating
    {
        void BeginErrorSession();
        bool EndErrorSession();
        string PreprocessTemplate(string inputFile, string content, ITextTemplatingCallback callback, string className, string classNamespace, out string[] references);
        string ProcessTemplate(string inputFile, string content, ITextTemplatingCallback callback = new ITextTemplatingCallback(), object hierarchy = new object());
    }
}

