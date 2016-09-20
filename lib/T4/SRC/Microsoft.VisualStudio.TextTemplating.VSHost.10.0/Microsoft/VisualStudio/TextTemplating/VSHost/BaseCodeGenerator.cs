namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public abstract class BaseCodeGenerator : Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator, IDisposable
    {
        private string codeFileNamespace = string.Empty;
        private string codeFilePath = string.Empty;
        private Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress codeGeneratorProgress;

        protected BaseCodeGenerator()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.codeGeneratorProgress = null;
        }

        ~BaseCodeGenerator()
        {
            this.Dispose(false);
        }

        public void Generate(string inputFilePath, string inputFileContents, string defaultNamespace, out IntPtr outputFileContents, out int output, Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress generateProgress)
        {
            if (inputFileContents == null)
            {
                throw new ArgumentNullException("inputFileContents");
            }
            try
            {
                this.codeFilePath = inputFilePath;
                this.codeFileNamespace = defaultNamespace;
                this.codeGeneratorProgress = generateProgress;
                byte[] source = this.GenerateCode(inputFilePath, inputFileContents);
                if (source == null)
                {
                    outputFileContents = IntPtr.Zero;
                    output = 0;
                }
                else
                {
                    output = source.Length;
                    outputFileContents = Marshal.AllocCoTaskMem(output);
                    Marshal.Copy(source, 0, outputFileContents, output);
                }
            }
            finally
            {
                this.codeFilePath = null;
                this.codeFileNamespace = null;
                this.codeGeneratorProgress = null;
            }
        }

        protected abstract byte[] GenerateCode(string inputFileName, string inputFileContent);
        protected virtual void GeneratorErrorCallback(bool warning, int level, string message, int line, int column)
        {
            Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress codeGeneratorProgress = this.CodeGeneratorProgress;
            if (codeGeneratorProgress != null)
            {
                if (line > 0)
                {
                    line--;
                }
                if (column > 0)
                {
                    column--;
                }
                ErrorHandler.ThrowOnFailure(codeGeneratorProgress.GeneratorError(warning ? -1 : 0, (uint) level, message, (uint) line, (uint) column));
            }
        }

        public abstract string GetDefaultExtension();
        int Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
        {
            try
            {
                pbstrDefaultExtension = this.GetDefaultExtension();
            }
            catch
            {
                pbstrDefaultExtension = string.Empty;
                return -2147467259;
            }
            return 0;
        }

        int Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                IntPtr zero = IntPtr.Zero;
                int output = 0;
                this.Generate(wszInputFilePath, bstrInputFileContents, wszDefaultNamespace, out zero, out output, pGenerateProgress);
                rgbOutputFileContents[0] = zero;
                pcbOutput = (uint) output;
            }
            catch
            {
                pcbOutput = 0;
                rgbOutputFileContents[0] = IntPtr.Zero;
                return -2147467259;
            }
            return 0;
        }

        internal Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress CodeGeneratorProgress
        {
            [DebuggerStepThrough]
            get
            {
                return this.codeGeneratorProgress;
            }
        }

        protected string FileNamespace
        {
            [DebuggerStepThrough]
            get
            {
                return this.codeFileNamespace;
            }
        }

        protected string InputFilePath
        {
            [DebuggerStepThrough]
            get
            {
                return this.codeFilePath;
            }
        }
    }
}

