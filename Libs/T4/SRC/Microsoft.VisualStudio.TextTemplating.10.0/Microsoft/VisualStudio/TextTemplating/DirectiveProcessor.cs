namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public abstract class DirectiveProcessor
    {
        private CompilerErrorCollection compilerErrors;

        protected DirectiveProcessor()
        {
        }

        public abstract void FinishProcessingRun();
        public abstract string GetClassCodeForProcessingRun();
        public abstract string[] GetImportsForProcessingRun();
        public abstract string GetPostInitializationCodeForProcessingRun();
        public abstract string GetPreInitializationCodeForProcessingRun();
        public abstract string[] GetReferencesForProcessingRun();
        public virtual void Initialize(ITextTemplatingEngineHost host)
        {
        }

        public abstract bool IsDirectiveSupported(string directiveName);
        public abstract void ProcessDirective(string directiveName, IDictionary<string, string> arguments);
        public virtual void StartProcessingRun(CodeDomProvider languageProvider, string templateContents, CompilerErrorCollection errors)
        {
            this.compilerErrors = errors;
        }

        protected CompilerErrorCollection Errors
        {
            get
            {
                return this.compilerErrors;
            }
        }
    }
}

