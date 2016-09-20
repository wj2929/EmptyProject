namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Runtime.InteropServices;

    [CLSCompliant(true)]
    public interface ITextTemplatingEngine
    {
        string PreprocessTemplate(string content, ITextTemplatingEngineHost host, string className, string classNamespace, out string language, out string[] references);
        string ProcessTemplate(string content, ITextTemplatingEngineHost host);
    }
}

