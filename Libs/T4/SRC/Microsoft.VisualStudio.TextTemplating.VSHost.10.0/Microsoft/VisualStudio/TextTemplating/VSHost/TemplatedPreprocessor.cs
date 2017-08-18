namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using EnvDTE;
    using Microsoft.CSharp;
    using Microsoft.VisualBasic;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Designer.Interfaces;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.CodeDom.Compiler;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    [Guid("F56DB4B6-C280-40f1-855D-5DA0ED7BD270")]
    public class TemplatedPreprocessor : BaseTemplatedCodeGenerator
    {
        private string CreateValidIdentifier(IVsHierarchy hierarchy, string originalFileName)
        {
            CodeDomProvider codeDomProvider = null;
            IVSMDCodeDomProvider service = base.GetService(typeof(SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;
            if (service != null)
            {
                codeDomProvider = (CodeDomProvider) service.CodeDomProvider;
            }
            if (codeDomProvider == null)
            {
                string kind = ToDteProject(hierarchy).Kind;
                if (StringComparer.OrdinalIgnoreCase.Compare("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", kind) == 0)
                {
                    codeDomProvider = new CSharpCodeProvider();
                }
                else
                {
                    codeDomProvider = new VBCodeProvider();
                }
            }
            if (codeDomProvider != null)
            {
                return codeDomProvider.CreateValidIdentifier(originalFileName);
            }
            return originalFileName;
        }

        private string MakeClassName(string potentialClassName, IVsHierarchy hierarchy)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(potentialClassName);
            potentialClassName = this.CreateValidIdentifier(hierarchy, fileNameWithoutExtension);
            if ((char.IsUpper(fileNameWithoutExtension, 0) && char.IsLower(potentialClassName, 0)) && (char.ToLower(fileNameWithoutExtension[0], CultureInfo.InvariantCulture) == char.ToLower(potentialClassName[0], CultureInfo.InvariantCulture)))
            {
                potentialClassName = char.ToUpperInvariant(potentialClassName[0]) + potentialClassName.Substring(1);
            }
            return potentialClassName;
        }

        protected override string ProcessTemplate(string inputFileName, string inputFileContent, ITextTemplating processor, IVsHierarchy hierarchy)
        {
            string[] strArray;
            return processor.PreprocessTemplate(inputFileName, inputFileContent, this, this.MakeClassName(inputFileName, hierarchy), base.FileNamespace, out strArray);
        }

        private static Project ToDteProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
            {
                throw new ArgumentNullException("hierarchy");
            }
            object pvar = null;
            ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(0xfffffffe, -2027, out pvar));
            return (Project) pvar;
        }
    }
}

