namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextTemplating;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Messaging;
    using System.Text;

    public abstract class BaseTemplatedCodeGenerator : BaseCodeGeneratorWithSite, ITextTemplatingCallback
    {
        private bool encodingSetFromOutputDirective;
        private bool errors;
        private string extension = ".cs";
        private Encoding outputEncoding;

        protected BaseTemplatedCodeGenerator()
        {
        }

        public void ErrorCallback(bool warning, string message, int line, int column)
        {
            this.errors = true;
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            if (this.ShowWarningDialog())
            {
                return new byte[0];
            }
            base.SetWaitCursor();
            this.errors = false;
            this.encodingSetFromOutputDirective = false;
            this.outputEncoding = null;
            ITextTemplating textTemplating = this.TextTemplating;
            string s = string.Empty;
            CallContext.LogicalSetData("NamespaceHint", base.FileNamespace);
            try
            {
                if (textTemplating == null)
                {
                    throw new InvalidOperationException(Resources.TextTemplatingUnavailable);
                }
                textTemplating.BeginErrorSession();
                IVsHierarchy service = base.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                s = this.ProcessTemplate(inputFileName, inputFileContent, textTemplating, service);
                this.errors |= textTemplating.EndErrorSession();
                MarkProjectForTextTemplating(service);
            }
            finally
            {
                CallContext.FreeNamedDataSlot("NamespaceHint");
            }
            if (this.errors)
            {
                IVsErrorList errorList = base.ErrorList;
                if (errorList != null)
                {
                    try
                    {
                        errorList.BringToFront();
                        errorList.ForceShowErrors();
                    }
                    catch
                    {
                    }
                }
            }
            if (this.outputEncoding == null)
            {
                this.outputEncoding = EncodingHelper.GetEncoding(inputFileName);
            }
            if (this.outputEncoding == null)
            {
                this.outputEncoding = Encoding.UTF8;
            }
            byte[] bytes = this.outputEncoding.GetBytes(s);
            byte[] preamble = this.outputEncoding.GetPreamble();
            if ((preamble != null) && (preamble.Length > 0))
            {
                bool flag2 = false;
                if (bytes.Length >= preamble.Length)
                {
                    flag2 = true;
                    for (int i = 0; i < preamble.Length; i++)
                    {
                        if (preamble[i] != bytes[i])
                        {
                            flag2 = false;
                            break;
                        }
                    }
                }
                if (!flag2)
                {
                    byte[] array = new byte[preamble.Length + bytes.Length];
                    preamble.CopyTo(array, 0);
                    bytes.CopyTo(array, preamble.Length);
                    bytes = array;
                }
            }
            return bytes;
        }

        public override string GetDefaultExtension()
        {
            return this.extension;
        }

        private static void MarkProjectForTextTemplating(IVsHierarchy hierarchy)
        {
            Guid gUID = typeof(STextTemplating).GUID;
            IVsProjectStartupServices startUpServices = OrchestratorCommandSet.GetStartUpServices(hierarchy);
            if ((startUpServices != null) && !OrchestratorCommandSet.StartupServicesReferencesService(startUpServices, gUID))
            {
                ErrorHandler.Failed(startUpServices.AddStartupService(ref gUID));
            }
        }

        protected virtual string ProcessTemplate(string inputFileName, string inputFileContent, ITextTemplating processor, IVsHierarchy hierarchy)
        {
            return processor.ProcessTemplate(inputFileName, inputFileContent, this, hierarchy);
        }

        public void SetFileExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("extension");
            }
            this.extension = extension;
        }

        public virtual void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            if (!this.encodingSetFromOutputDirective)
            {
                if (fromOutputDirective)
                {
                    this.encodingSetFromOutputDirective = true;
                    this.outputEncoding = encoding;
                }
                else
                {
                    if ((this.outputEncoding != null) && (encoding != this.outputEncoding))
                    {
                        this.outputEncoding = Encoding.UTF8;
                    }
                    this.outputEncoding = encoding;
                }
            }
        }

        private bool ShowWarningDialog()
        {
            return (OrchestratorPackage.ShowSecurityDialogDuringBatchRun && OrchestratorOptionsAutomation.ShowSecurityWarningDialog(base.GlobalServiceProvider));
        }

        [CLSCompliant(false)]
        protected ITextTemplating TextTemplating
        {
            get
            {
                ITextTemplating objectForIUnknown = null;
                IVsHierarchy service = base.GetService(typeof(IVsHierarchy)) as IVsHierarchy;
                if (service != null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP = null;
                    if (!Microsoft.VisualStudio.TextTemplating.NativeMethods.Failed(service.GetSite(out ppSP)) && (ppSP != null))
                    {
                        Guid gUID = typeof(STextTemplating).GUID;
                        IntPtr zero = IntPtr.Zero;
                        ErrorHandler.ThrowOnFailure(ppSP.QueryService(ref gUID, ref gUID, out zero));
                        if (zero != IntPtr.Zero)
                        {
                            objectForIUnknown = Marshal.GetObjectForIUnknown(zero) as ITextTemplating;
                        }
                    }
                }
                return objectForIUnknown;
            }
        }
    }
}

