namespace Microsoft.VisualStudio.TextTemplating.Modeling
{
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Modeling.Integration;
    using System;
    using System.Diagnostics;

    internal class VsTextTemplatingModelingDocumentHandler : ModelingDocumentHandler
    {
        private ModelElement root;

        internal VsTextTemplatingModelingDocumentHandler(string modelFile, ModelElement root) : base(modelFile)
        {
            if (modelFile == null)
            {
                throw new ArgumentNullException("modelFile");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            this.root = root;
        }

        public override ModelElement Root
        {
            [DebuggerStepThrough]
            get
            {
                return this.root;
            }
        }
    }
}

