namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    public abstract class RequiresProvidesDirectiveProcessor : DirectiveProcessor
    {
        private StringBuilder codeBuffer;
        private CodeDomProvider languageCodeDomProvider;
        private StringBuilder postInitializationBuffer;
        private StringBuilder preInitializationBuffer;
        private ProcessorState state;
        private ITextTemplatingEngineHost templatingHost;

        protected RequiresProvidesDirectiveProcessor()
        {
        }

        public override void FinishProcessingRun()
        {
            this.state = ProcessorState.PreStart;
        }

        protected abstract void GeneratePostInitializationCode(string directiveName, StringBuilder codeBuffer, CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments);
        protected abstract void GeneratePreInitializationCode(string directiveName, StringBuilder codeBuffer, CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments);
        protected abstract void GenerateTransformCode(string directiveName, StringBuilder codeBuffer, CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments);
        public override string GetClassCodeForProcessingRun()
        {
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.GetClassCodeCallError);
            }
            return this.codeBuffer.ToString();
        }

        public override string[] GetImportsForProcessingRun()
        {
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.GetImportsCallError);
            }
            return new string[0];
        }

        public override string GetPostInitializationCodeForProcessingRun()
        {
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.GetClassCodeCallError);
            }
            return this.postInitializationBuffer.ToString();
        }

        public override string GetPreInitializationCodeForProcessingRun()
        {
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.GetClassCodeCallError);
            }
            return this.preInitializationBuffer.ToString();
        }

        public override string[] GetReferencesForProcessingRun()
        {
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.GetReferencesCallError);
            }
            return new string[0];
        }

        public override void Initialize(ITextTemplatingEngineHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            base.Initialize(host);
            this.state = ProcessorState.PreStart;
            this.templatingHost = host;
        }

        protected abstract void InitializeProvidesDictionary(string directiveName, IDictionary<string, string> providesDictionary);
        protected abstract void InitializeRequiresDictionary(string directiveName, IDictionary<string, string> requiresDictionary);
        private static IDictionary<string, string> ParseArgument(string argument)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string str in argument.Split(new char[] { ';' }))
            {
                if (str.Contains("="))
                {
                    string[] strArray2 = str.Split(new char[] { '=' });
                    if (strArray2.Length != 2)
                    {
                        throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidDirectiveArgumentFormat, new object[] { str }));
                    }
                    dictionary.Add(strArray2[0], strArray2[1]);
                }
                else
                {
                    dictionary.Add(str, null);
                }
            }
            return dictionary;
        }

        protected virtual void PostProcessArguments(string directiveName, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments)
        {
        }

        private static void ProcessArgument(string directiveName, IDictionary<string, string> arguments, string argumentName, IDictionary<string, string> argumentDictionary, bool mandatory)
        {
            string str;
            bool flag = arguments.TryGetValue(argumentName, out str);
            if (mandatory && !flag)
            {
                throw new DirectiveProcessorException(string.Format(CultureInfo.CurrentCulture, Resources.NotEnoughDirectiveParameters, new object[] { argumentName, directiveName }));
            }
            if (flag)
            {
                IDictionary<string, string> dictionary = ParseArgument(str);
                foreach (string str2 in dictionary.Keys)
                {
                    if (!argumentDictionary.ContainsKey(str2))
                    {
                        throw new DirectiveProcessorException(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedArgumentValue, new object[] { str2, argumentName, directiveName }));
                    }
                    string str3 = dictionary[str2];
                    if (!string.IsNullOrEmpty(str3))
                    {
                        argumentDictionary[str2] = StripQuotes(str3);
                    }
                }
            }
        }

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            if (directiveName == null)
            {
                throw new ArgumentNullException("directiveName");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            if (this.state != ProcessorState.Started)
            {
                throw new InvalidOperationException(Resources.ProcessDirectiveCallError);
            }
            if (!this.IsDirectiveSupported(directiveName))
            {
                throw new DirectiveProcessorException(string.Format(CultureInfo.CurrentCulture, Resources.DirectiveNotSupported, new object[] { directiveName }));
            }
            Dictionary<string, string> providesDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.InitializeProvidesDictionary(directiveName, providesDictionary);
            ProcessArgument(directiveName, arguments, "provides", providesDictionary, false);
            Dictionary<string, string> requiresDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.InitializeRequiresDictionary(directiveName, requiresDictionary);
            Dictionary<string, string> dictionary3 = new Dictionary<string, string>(requiresDictionary);
            List<string> list = new List<string>(requiresDictionary.Keys);
            foreach (string str in list)
            {
                requiresDictionary[str] = null;
            }
            ProcessArgument(directiveName, arguments, "requires", requiresDictionary, true);
            string directiveId = this.ProvideUniqueId(directiveName, arguments, requiresDictionary, providesDictionary);
            List<string> list2 = new List<string>();
            foreach (string str3 in requiresDictionary.Keys)
            {
                if (requiresDictionary[str3] == null)
                {
                    list2.Add(str3);
                }
            }
            foreach (string str4 in list2)
            {
                string str5 = this.Host.ResolveParameterValue(directiveId, this.FriendlyName, str4);
                if (string.IsNullOrEmpty(str5))
                {
                    if (dictionary3[str4] == null)
                    {
                        throw new DirectiveProcessorException(string.Format(CultureInfo.CurrentCulture, Resources.CannotResolveRequiresParameter, new object[] { str4, directiveName }));
                    }
                    requiresDictionary[str4] = dictionary3[str4];
                    continue;
                }
                requiresDictionary[str4] = str5;
            }
            this.PostProcessArguments(directiveName, requiresDictionary, providesDictionary);
            this.GeneratePreInitializationCode(directiveName, this.preInitializationBuffer, this.languageCodeDomProvider, requiresDictionary, providesDictionary);
            this.GeneratePostInitializationCode(directiveName, this.postInitializationBuffer, this.languageCodeDomProvider, requiresDictionary, providesDictionary);
            this.GenerateTransformCode(directiveName, this.codeBuffer, this.languageCodeDomProvider, requiresDictionary, providesDictionary);
        }

        private static string ProcessIdArgument(IDictionary<string, string> arguments)
        {
            string str;
            if (!arguments.TryGetValue("id", out str))
            {
                str = string.Empty;
            }
            return str;
        }

        protected virtual string ProvideUniqueId(string directiveName, IDictionary<string, string> arguments, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments)
        {
            if (directiveName == null)
            {
                throw new ArgumentNullException("directiveName");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            if (providesArguments == null)
            {
                throw new ArgumentNullException("providesArguments");
            }
            string str = ProcessIdArgument(arguments);
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (providesArguments.Count > 0)
            {
                IEnumerator<string> enumerator = providesArguments.Keys.GetEnumerator();
                enumerator.MoveNext();
                return providesArguments[enumerator.Current];
            }
            return directiveName;
        }

        public override void StartProcessingRun(CodeDomProvider languageProvider, string templateContents, CompilerErrorCollection errors)
        {
            if (languageProvider == null)
            {
                throw new ArgumentNullException("languageProvider");
            }
            base.StartProcessingRun(languageProvider, templateContents, errors);
            if (this.state != ProcessorState.PreStart)
            {
                throw new InvalidOperationException(Resources.StartProcessingCallError);
            }
            this.state = ProcessorState.Started;
            this.languageCodeDomProvider = languageProvider;
            this.codeBuffer = new StringBuilder();
            this.preInitializationBuffer = new StringBuilder();
            this.postInitializationBuffer = new StringBuilder();
        }

        private static string StripQuotes(string text)
        {
            return text.Trim().Trim(new char[] { '\'' });
        }

        protected abstract string FriendlyName { get; }

        protected ITextTemplatingEngineHost Host
        {
            [DebuggerStepThrough]
            get
            {
                return this.templatingHost;
            }
        }

        private enum ProcessorState
        {
            PreStart,
            Started
        }
    }
}

