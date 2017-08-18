namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    [CLSCompliant(true)]
    public interface ITextTemplatingEngineHost
    {
        object GetHostOption(string optionName);
        bool LoadIncludeText(string requestFileName, out string content, out string location);
        void LogErrors(CompilerErrorCollection errors);
        AppDomain ProvideTemplatingAppDomain(string content);
        string ResolveAssemblyReference(string assemblyReference);
        Type ResolveDirectiveProcessor(string processorName);
        string ResolveParameterValue(string directiveId, string processorName, string parameterName);
        string ResolvePath(string path);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);

        IList<string> StandardAssemblyReferences { get; }

        IList<string> StandardImports { get; }

        string TemplateFile { get; }
    }
}

