namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class Directive
    {
        private Microsoft.VisualStudio.TextTemplating.Block block;
        private string directiveName;
        private const string DirectiveProcessorAttributeName = "processor";
        private string directiveProcessorName;
        private IDictionary<string, string> parameters;

        public Directive(string directiveName, IDictionary<string, string> parameters, Microsoft.VisualStudio.TextTemplating.Block block)
        {
            this.directiveName = directiveName;
            this.parameters = parameters;
            this.block = block;
        }

        internal void SetDirectiveProcessorName(string processorName)
        {
            this.directiveProcessorName = processorName;
        }

        public Microsoft.VisualStudio.TextTemplating.Block Block
        {
            [DebuggerStepThrough]
            get
            {
                return this.block;
            }
        }

        public string DirectiveName
        {
            [DebuggerStepThrough]
            get
            {
                return this.directiveName;
            }
        }

        public string DirectiveProcessorName
        {
            get
            {
                if (string.IsNullOrEmpty(this.directiveProcessorName))
                {
                    this.Parameters.TryGetValue("processor", out this.directiveProcessorName);
                }
                return this.directiveProcessorName;
            }
        }

        public IDictionary<string, string> Parameters
        {
            [DebuggerStepThrough]
            get
            {
                return this.parameters;
            }
        }
    }
}

