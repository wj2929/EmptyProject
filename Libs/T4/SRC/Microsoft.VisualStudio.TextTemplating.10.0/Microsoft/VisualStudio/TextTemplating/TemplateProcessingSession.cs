namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.CSharp;
    using Microsoft.VisualBasic;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    [Serializable]
    internal sealed class TemplateProcessingSession : IDisposable
    {
        private List<string> assemblyDirectives = new List<string>();
        private string baseClassName;
        private bool cacheAssemblies;
        private string className;
        [NonSerialized]
        private System.CodeDom.Compiler.CodeDomProvider codeDomProvider;
        private string compilerOptions;
        private bool debug;
        private CultureInfo formatProvider;
        private bool hostSpecific;
        private List<string> importDirectives = new List<string>();
        private Stack<string> includeStack = new Stack<string>();
        private SupportedLanguages language;
        private Dictionary<string, string> languageOptions = new Dictionary<string, string>();
        private bool preprocess;
        private bool processedOutputDirective;
        private bool processedTemplateDirective;
        private string templateContents;
        private string templateFile;
        private ITextTemplatingSession userTransformationSession;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose && (this.codeDomProvider != null))
            {
                this.codeDomProvider.Dispose();
                this.codeDomProvider = null;
            }
        }

        ~TemplateProcessingSession()
        {
            this.Dispose(false);
        }

        public List<string> AssemblyDirectives
        {
            [DebuggerStepThrough]
            get
            {
                return this.assemblyDirectives;
            }
        }

        public string BaseClassName
        {
            [DebuggerStepThrough]
            get
            {
                return this.baseClassName;
            }
            [DebuggerStepThrough]
            set
            {
                this.baseClassName = value;
            }
        }

        public bool CacheAssemblies
        {
            get
            {
                return this.cacheAssemblies;
            }
            set
            {
                this.cacheAssemblies = value;
            }
        }

        public string ClassFullName
        {
            [DebuggerStepThrough]
            get
            {
                return this.className;
            }
            [DebuggerStepThrough]
            set
            {
                this.className = value;
            }
        }

        public System.CodeDom.Compiler.CodeDomProvider CodeDomProvider
        {
            [DebuggerStepThrough]
            get
            {
                if (this.codeDomProvider == null)
                {
                    if (this.Language == SupportedLanguages.VB)
                    {
                        this.codeDomProvider = new VBCodeProvider(this.LanguageOptions);
                    }
                    else
                    {
                        this.codeDomProvider = new CSharpCodeProvider(this.LanguageOptions);
                    }
                }
                return this.codeDomProvider;
            }
        }

        public string CompilerOptions
        {
            get
            {
                return this.compilerOptions;
            }
            set
            {
                this.compilerOptions = value;
            }
        }

        public bool Debug
        {
            [DebuggerStepThrough]
            get
            {
                return this.debug;
            }
            [DebuggerStepThrough]
            set
            {
                this.debug = value;
            }
        }

        public CultureInfo FormatProvider
        {
            [DebuggerStepThrough]
            get
            {
                if (this.formatProvider == null)
                {
                    this.formatProvider = CultureInfo.InvariantCulture;
                }
                return this.formatProvider;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    this.formatProvider = value;
                }
            }
        }

        public bool HostSpecific
        {
            [DebuggerStepThrough]
            get
            {
                return this.hostSpecific;
            }
            [DebuggerStepThrough]
            set
            {
                this.hostSpecific = value;
            }
        }

        public List<string> ImportDirectives
        {
            [DebuggerStepThrough]
            get
            {
                return this.importDirectives;
            }
        }

        public Stack<string> IncludeStack
        {
            [DebuggerStepThrough]
            get
            {
                return this.includeStack;
            }
        }

        public SupportedLanguages Language
        {
            [DebuggerStepThrough]
            get
            {
                return this.language;
            }
            [DebuggerStepThrough]
            set
            {
                this.language = value;
            }
        }

        public IDictionary<string, string> LanguageOptions
        {
            [DebuggerStepThrough]
            get
            {
                return this.languageOptions;
            }
        }

        public bool Preprocess
        {
            [DebuggerStepThrough]
            get
            {
                return this.preprocess;
            }
            [DebuggerStepThrough]
            set
            {
                this.preprocess = value;
            }
        }

        public bool ProcessedOutputDirective
        {
            [DebuggerStepThrough]
            get
            {
                return this.processedOutputDirective;
            }
            [DebuggerStepThrough]
            set
            {
                this.processedOutputDirective = value;
            }
        }

        public bool ProcessedTemplateDirective
        {
            [DebuggerStepThrough]
            get
            {
                return this.processedTemplateDirective;
            }
            [DebuggerStepThrough]
            set
            {
                this.processedTemplateDirective = value;
            }
        }

        public string TemplateContents
        {
            [DebuggerStepThrough]
            get
            {
                return this.templateContents;
            }
            [DebuggerStepThrough]
            set
            {
                this.templateContents = value;
            }
        }

        public string TemplateFile
        {
            [DebuggerStepThrough]
            get
            {
                return this.templateFile;
            }
            [DebuggerStepThrough]
            set
            {
                this.templateFile = value;
            }
        }

        public ITextTemplatingSession UserTransformationSession
        {
            get
            {
                return this.userTransformationSession;
            }
            set
            {
                this.userTransformationSession = value;
            }
        }
    }
}

