namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.ComponentModel.Design;
    using System.Runtime.InteropServices;

    [ProvideCodeGeneratorExtension("TextTemplatingFileGenerator", ".tt", ProjectSystem="{E24C65DC-7377-472b-9ABA-BC803B73C61A}", ProjectSystemPackage="{39c9c826-8ef8-4079-8c95-428f5b1c323f}"), ProvideCodeGenerator(typeof(TemplatedCodeGenerator), "TextTemplatingFileGenerator", "Generator that uses the Text Templating engine", true, ProjectSystem="{164b10b9-b200-11d0-8c61-00a0c91e29d5}"), ProvideCodeGenerator(typeof(TemplatedCodeGenerator), "TextTemplatingFileGenerator", "Generator that uses the Text Templating engine", true, ProjectSystem="{39c9c826-8ef8-4079-8c95-428f5b1c323f}"), ProvideCodeGenerator(typeof(TemplatedPreprocessor), "TextTemplatingFilePreprocessor", "Generator that uses the Text Templating engine to preprocess a template", true, ProjectSystem="{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"), ProvideCodeGenerator(typeof(TemplatedPreprocessor), "TextTemplatingFilePreprocessor", "Generator that uses the Text Templating engine to preprocess a template", true, ProjectSystem="{164b10b9-b200-11d0-8c61-00a0c91e29d5}"), ProvideCodeGenerator(typeof(TemplatedPreprocessor), "TextTemplatingFilePreprocessor", "Generator that uses the Text Templating engine to preprocess a template", true, ProjectSystem="{39c9c826-8ef8-4079-8c95-428f5b1c323f}"), ProvideCodeGeneratorExtension("TextTemplatingFileGenerator", ".tt", ProjectSystem="{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}", ProjectSystemPackage="{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"), ProvideCodeGeneratorExtension("TextTemplatingFileGenerator", ".tt", ProjectSystem="{F184B08F-C81C-45F6-A57F-5ABD9991F28F}", ProjectSystemPackage="{164b10b9-b200-11d0-8c61-00a0c91e29d5}"), PackageRegistration(RegisterUsing=RegistrationMethod.Assembly, UseManagedResourcesOnly=true), Guid("A9696DE6-E209-414d-BBEC-A0506FB0E924"), DefaultRegistryRoot(@"Software\Microsoft\VisualStudio\10.0"), ProvideMenuResource("1000.ctmenu", 13), ProvideOptionPage(typeof(OrchestratorOptionsPage), "DSLTools", "TextTemplating", 0x1f5, 0x1f6, false), ProvideService(typeof(STextTemplating)), ProvideCodeGenerator(typeof(TemplatedCodeGenerator), "TextTemplatingFileGenerator", "Generator that uses the Text Templating engine", true, ProjectSystem="{fae04ec1-301f-11d3-bf4b-00c04f79efbc}")]
    internal sealed class OrchestratorPackage : Package
    {
        private OrchestratorOptionsAutomation optionsAutomation;
        private const string RegistryKeyName = "DSLTools";
        private static bool showSecurityDialogDuringBatchRun = true;
        private static OrchestratorPackage singletonInstance;
        private SolutionEvents solutionEvents;
        internal const string TemplatingGeneratorName = "TextTemplatingFileGenerator";
        internal const string TemplatingPreprocessorName = "TextTemplatingFilePreprocessor";
        private TextTemplatingService textTemplatingService;

        public OrchestratorPackage()
        {
            singletonInstance = this;
        }

        internal void BeginErrorSession()
        {
            if (this.textTemplatingService == null)
            {
                this.CreateTextTemplatingService(this);
            }
            this.textTemplatingService.BeginErrorSession();
        }

        internal void ClearTextTemplatingServiceErrorStatus()
        {
            if (this.textTemplatingService != null)
            {
                this.textTemplatingService.LastInvocationRaisedErrors = false;
            }
        }

        private void CreateTextTemplatingService(OrchestratorPackage package)
        {
            this.textTemplatingService = new TextTemplatingService(package);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    singletonInstance = null;
                    ((IServiceContainer) this).RemoveService(typeof(STextTemplating), true);
                    if (this.solutionEvents != null)
                    {
                        this.solutionEvents.remove_AfterClosing(new _dispSolutionEvents_AfterClosingEventHandler(this.OnSolutionClose));
                        this.solutionEvents = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        internal void EndErrorSession()
        {
            if (this.textTemplatingService != null)
            {
                this.textTemplatingService.EndErrorSession();
            }
        }

        internal bool FindTextTemplatingServiceErrorStatus()
        {
            return ((this.textTemplatingService != null) && this.textTemplatingService.LastInvocationRaisedErrors);
        }

        internal DTE GetDTE()
        {
            return (base.GetService(typeof(DTE)) as DTE);
        }

        protected override void Initialize()
        {
            base.Initialize();
            new OrchestratorCommandSet(this);
            this.optionsAutomation = new OrchestratorOptionsAutomation(this);
            ServiceCreatorCallback callback = new ServiceCreatorCallback(this.OnCreateService);
            ((IServiceContainer) this).AddService(typeof(STextTemplating), callback, true);
            DTE dTE = this.GetDTE();
            if (dTE != null)
            {
                this.solutionEvents = dTE.Events.SolutionEvents;
                if (this.solutionEvents != null)
                {
                    this.solutionEvents.add_AfterClosing(new _dispSolutionEvents_AfterClosingEventHandler(this.OnSolutionClose));
                }
            }
        }

        private object OnCreateService(IServiceContainer container, Type serviceType)
        {
            if (!(serviceType == typeof(STextTemplating)))
            {
                return null;
            }
            if (this.textTemplatingService == null)
            {
                this.CreateTextTemplatingService(this);
            }
            return this.textTemplatingService;
        }

        private void OnSolutionClose()
        {
            if (this.textTemplatingService != null)
            {
                this.textTemplatingService.UnloadGenerationAppDomain();
            }
        }

        public OrchestratorOptionsAutomation OptionsAutomation
        {
            get
            {
                return this.optionsAutomation;
            }
        }

        internal static bool ShowSecurityDialogDuringBatchRun
        {
            get
            {
                return showSecurityDialogDuringBatchRun;
            }
            set
            {
                showSecurityDialogDuringBatchRun = value;
            }
        }

        internal static OrchestratorPackage Singleton
        {
            get
            {
                return singletonInstance;
            }
        }
    }
}

