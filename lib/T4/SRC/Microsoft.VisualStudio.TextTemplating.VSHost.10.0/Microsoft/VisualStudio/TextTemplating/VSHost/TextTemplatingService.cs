namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.ExtensionManager;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.TextTemplating;
    using Microsoft.Win32;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Messaging;
    using System.Security;
    using System.Security.Policy;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using VSLangProj;

    internal sealed class TextTemplatingService : MarshalByRefObject, STextTemplating, ITextTemplating, ITextTemplatingComponents, ITextTemplatingSessionHost, ITextTemplatingEngineHost, System.IServiceProvider, Microsoft.VisualStudio.OLE.Interop.IServiceProvider, IDisposable
    {
        private ITextTemplatingCallback callback;
        private Dictionary<string, int> currentErrors;
        private const string DefaultFileExtension = ".cs";
        private DTE dte;
        private ITextTemplatingEngine engine;
        private ErrorListProvider errorProvider;
        private int errorSessionDepth;
        private const int GeneratedDomainReuseLimit = 0x19;
        private IVsHierarchy hierarchy;
        public const string IncludeContentType = "Microsoft.T4.Include";
        private string inputFile;
        private const string installValueName = "InstallDir";
        private bool lastInvocationRaisedErrors;
        public const string NamespaceHintName = "NamespaceHint";
        private OrchestratorPackage package;
        private ServiceProvider serviceProvider;
        private ITextTemplatingSession transformationSession;
        private bool transformationSessionImplicitlyCreated;
        private AppDomain transformDomain;
        private int transformDomainUseCount;
        private static Regex vsMacroRegEx = new Regex(@" \$\(  (?<macroName>\w+) \) ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private const string vsMacroRegExPattern = @" \$\(  (?<macroName>\w+) \) ";

        internal TextTemplatingService(OrchestratorPackage package)
        {
            DTE dTE = package.GetDTE();
            this.package = package;
            this.engine = new Microsoft.VisualStudio.TextTemplating.Engine();
            this.serviceProvider = new ServiceProvider(dTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            this.errorProvider = new ErrorListProvider(this.serviceProvider);
            this.errorProvider.MaintainInitialTaskOrder = true;
            dTE.Events.SolutionEvents.add_AfterClosing(new _dispSolutionEvents_AfterClosingEventHandler(this.SolutionEvents_AfterClosing));
        }

        public void BeginErrorSession()
        {
            if (this.errorSessionDepth == 0)
            {
                if (this.currentErrors == null)
                {
                    this.currentErrors = new Dictionary<string, int>();
                }
                else
                {
                    this.currentErrors.Clear();
                }
                this.errorProvider.Tasks.Clear();
                if ((this.transformationSession == null) && !this.transformationSessionImplicitlyCreated)
                {
                    this.transformationSession = new TextTemplatingSession();
                    this.transformationSessionImplicitlyCreated = true;
                }
            }
            this.errorSessionDepth++;
        }

        private static bool CheckSecurityZone(string path)
        {
            Zone zone = Zone.CreateFromUrl(new Uri(path).AbsoluteUri);
            if (zone.SecurityZone != SecurityZone.MyComputer)
            {
                return (zone.SecurityZone == SecurityZone.Trusted);
            }
            return true;
        }

        public ITextTemplatingSession CreateSession()
        {
            return new TextTemplatingSession();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.package.GetDTE().Events.SolutionEvents.remove_AfterClosing(new _dispSolutionEvents_AfterClosingEventHandler(this.SolutionEvents_AfterClosing));
                this.errorProvider.Dispose();
                this.serviceProvider.Dispose();
            }
        }

        public bool EndErrorSession()
        {
            this.errorSessionDepth--;
            if ((this.errorSessionDepth == 0) && this.transformationSessionImplicitlyCreated)
            {
                this.transformationSession = null;
                this.transformationSessionImplicitlyCreated = false;
            }
            bool flag = false;
            if (this.currentErrors != null)
            {
                flag = this.currentErrors.Count > 0;
            }
            return flag;
        }

        private string ExpandAllVariables(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = ExpandEnvironmentVariables(input);
                input = this.ExpandVsMacroVariables(input);
            }
            return input;
        }

        private static string ExpandEnvironmentVariables(string path)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            return path;
        }

        private string ExpandVsMacroVariables(string input)
        {
            IVsBuildMacroInfo buildMacroInfo = this.hierarchy as IVsBuildMacroInfo;
            if (buildMacroInfo == null)
            {
                return input;
            }
            MatchEvaluator evaluator = delegate (Match m) {
                string str;
                Group group = m.Groups["macroName"];
                if ((group.Success && ErrorHandler.Succeeded(buildMacroInfo.GetBuildMacroValue(group.Value, out str))) && !string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return m.Value;
            };
            return vsMacroRegEx.Replace(input, evaluator);
        }

        ~TextTemplatingService()
        {
            this.Dispose(false);
        }

        private static bool GetBooleanOption(string optionValueName, bool defaultValue)
        {
            bool flag = defaultValue;
            string name = "TextTemplating";
            OrchestratorPackage singleton = OrchestratorPackage.Singleton;
            if (singleton != null)
            {
                using (RegistryKey key = singleton.ApplicationRegistryRoot.OpenSubKey(name, false))
                {
                    if (key != null)
                    {
                        object obj2 = key.GetValue(optionValueName, defaultValue);
                        if (obj2 != null)
                        {
                            flag = Convert.ToBoolean(obj2, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            return flag;
        }

        private static bool GetCacheAssembliesOption()
        {
            return GetBooleanOption("CacheAssemblies", true);
        }

        private void GetExtensionManagerIncludeFolders(List<string> includeFolders)
        {
            IVsExtensionManager service = this.serviceProvider.GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (service != null)
            {
                includeFolders.AddRange(service.GetEnabledExtensionContentLocations("Microsoft.T4.Include"));
            }
        }

        public object GetHostOption(string optionName)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(optionName, "CacheAssemblies") == 0)
            {
                return GetCacheAssembliesOption();
            }
            return null;
        }

        private void GetRegistryIncludeFolders(List<string> includeFolders)
        {
            string extension = Path.GetExtension(this.inputFile);
            if (!string.IsNullOrEmpty(extension))
            {
                OrchestratorPackage singleton = OrchestratorPackage.Singleton;
                if (singleton != null)
                {
                    string name = @"TextTemplating\IncludeFolders\" + extension;
                    using (RegistryKey key = singleton.ApplicationRegistryRoot.OpenSubKey(name, false))
                    {
                        if (key != null)
                        {
                            string[] valueNames = key.GetValueNames();
                            Array.Sort<string>(valueNames, StringComparer.OrdinalIgnoreCase);
                            foreach (string str3 in valueNames)
                            {
                                if (str3.StartsWith("include", StringComparison.OrdinalIgnoreCase))
                                {
                                    string str4 = key.GetValue(str3) as string;
                                    if (!string.IsNullOrEmpty(str4))
                                    {
                                        includeFolders.Add(str4);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public object GetService(Type serviceType)
        {
            if (this.serviceProvider == null)
            {
                return null;
            }
            return this.serviceProvider.GetService(serviceType);
        }

        private static bool GetShadowCopyOption()
        {
            return GetBooleanOption("ShadowCopy", true);
        }

        private static string GetVSInstallDir(RegistryKey applicationRoot)
        {
            string str = string.Empty;
            if (applicationRoot != null)
            {
                str = applicationRoot.GetValue("InstallDir") as string;
            }
            if (str == null)
            {
                return string.Empty;
            }
            return str.Replace(@"\\", @"\");
        }

        private static string LoadIncludeFileContent(string absolutePath)
        {
            using (StreamReader reader = new StreamReader(absolutePath))
            {
                return reader.ReadToEnd();
            }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            try
            {
                if (requestFileName != null)
                {
                    if (requestFileName.Length == 0)
                    {
                        content = string.Empty;
                        location = string.Empty;
                        return false;
                    }
                    requestFileName = this.ExpandAllVariables(requestFileName);
                    if (Path.IsPathRooted(requestFileName))
                    {
                        if (CheckSecurityZone(requestFileName))
                        {
                            content = LoadIncludeFileContent(requestFileName);
                            location = requestFileName;
                            return true;
                        }
                        this.LogError(false, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.IncludeDirectiveFailed, new object[] { string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.PathNotTrusted, new object[] { requestFileName }) }), -1, -1, this.inputFile);
                        content = string.Empty;
                        location = string.Empty;
                        return false;
                    }
                    List<string> includeFolders = new List<string>();
                    if (!string.IsNullOrEmpty(this.inputFile))
                    {
                        includeFolders.Add(Path.GetDirectoryName(this.inputFile));
                        this.GetRegistryIncludeFolders(includeFolders);
                        this.GetExtensionManagerIncludeFolders(includeFolders);
                    }
                    foreach (string str in includeFolders)
                    {
                        string path = Path.Combine(ExpandEnvironmentVariables(str), requestFileName);
                        if (!CheckSecurityZone(path))
                        {
                            this.LogError(true, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.PathNotTrusted, new object[] { path }), -1, -1, this.inputFile);
                        }
                        else if (File.Exists(path))
                        {
                            content = LoadIncludeFileContent(path);
                            location = path;
                            return true;
                        }
                    }
                }
                content = string.Empty;
                location = string.Empty;
                return false;
            }
            catch (FileNotFoundException)
            {
                this.LogError(false, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.IncludeDirectiveFailed, new object[] { requestFileName }), -1, -1, this.inputFile);
                content = string.Empty;
                location = string.Empty;
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                this.LogError(false, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.IncludeDirectiveFailed, new object[] { requestFileName }), -1, -1, this.inputFile);
                content = string.Empty;
                location = string.Empty;
                return false;
            }
        }

        private void LogError(bool isWarning, string message, int line, int column, string fileName)
        {
            if (line > 0)
            {
                line--;
            }
            if (column > 0)
            {
                column--;
            }
            int num = 0;
            if ((this.currentErrors == null) || !this.currentErrors.TryGetValue(message, out num))
            {
                if (this.currentErrors != null)
                {
                    this.currentErrors[message] = 1;
                }
                if (this.callback != null)
                {
                    this.callback.ErrorCallback(isWarning, message, line, column);
                }
                ErrorTask task = new ErrorTask {
                    Category = TaskCategory.BuildCompile,
                    Document = fileName,
                    CanDelete = false,
                    Column = column,
                    Line = line,
                    Text = message,
                    ErrorCategory = isWarning ? TaskErrorCategory.Warning : TaskErrorCategory.Error
                };
                task.Navigate += new EventHandler(this.task_Navigate);
                this.errorProvider.Tasks.Add(task);
            }
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            foreach (CompilerError error in errors)
            {
                if (!error.IsWarning)
                {
                    this.lastInvocationRaisedErrors = true;
                }
                this.LogError(error.IsWarning, error.ErrorText, error.Line, error.Column, error.FileName);
            }
        }

        int Microsoft.VisualStudio.OLE.Interop.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            object service = this.serviceProvider.GetService(guidService);
            if (service != null)
            {
                IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(service);
                if (iUnknownForObject != IntPtr.Zero)
                {
                    return Marshal.QueryInterface(iUnknownForObject, ref riid, out ppvObject);
                }
            }
            ppvObject = IntPtr.Zero;
            return -2147467259;
        }

        public string PreprocessTemplate(string templateInputFile, string templateContent, ITextTemplatingCallback processingCallback, string className, string classNamespace, out string[] references)
        {
            string str2;
            this.hierarchy = null;
            references = new string[0];
            this.callback = processingCallback;
            this.inputFile = templateInputFile;
            this.lastInvocationRaisedErrors = false;
            string str = "";
            str = this.engine.PreprocessTemplate(templateContent, this, className, classNamespace, out str2, out references);
            if (StringComparer.OrdinalIgnoreCase.Compare("CSharp", str2) == 0)
            {
                this.SetFileExtension(".cs");
                return str;
            }
            if (StringComparer.OrdinalIgnoreCase.Compare("VB", str2) == 0)
            {
                this.SetFileExtension(".vb");
                return str;
            }
            this.SetFileExtension(".cs");
            return str;
        }

        private static void ProcessProject(Project project, Dictionary<Project, string> projects, string parentName)
        {
            string str = string.IsNullOrEmpty(parentName) ? project.Name : (parentName + "/" + project.Name);
            if (project.Object is VSProject)
            {
                projects[project] = str;
            }
            else if (project.Object is SolutionFolder)
            {
                foreach (ProjectItem item in project.ProjectItems)
                {
                    Project project2 = item.Object as Project;
                    if (project2 != null)
                    {
                        ProcessProject(project2, projects, str);
                    }
                }
            }
        }

        public string ProcessTemplate(string templateInputFile, string templateContent, ITextTemplatingCallback processingCallback = new ITextTemplatingCallback(), object hierarchy = new object())
        {
            this.hierarchy = hierarchy as IVsHierarchy;
            this.callback = processingCallback;
            this.inputFile = templateInputFile;
            this.lastInvocationRaisedErrors = false;
            this.SetFileExtension(".cs");
            return this.engine.ProcessTemplate(templateContent, this);
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            bool flag = false;
            if (GetCacheAssembliesOption())
            {
                if (this.transformDomain != null)
                {
                    AssemblyCacheMonitor monitor = this.transformDomain.CreateInstanceAndUnwrap(typeof(Microsoft.VisualStudio.TextTemplating.Engine).Assembly.FullName, typeof(AssemblyCacheMonitor).FullName) as AssemblyCacheMonitor;
                    if ((monitor != null) && (monitor.GetStaleAssembliesCount(new TimeSpan(0, 15, 0)) >= 15))
                    {
                        flag = true;
                    }
                }
            }
            else if (this.transformDomainUseCount >= 0x19)
            {
                flag = true;
            }
            if ((this.transformDomain != null) && this.transformDomain.ShadowCopyFiles)
            {
                ShadowCopyMonitor monitor2 = this.transformDomain.CreateInstanceAndUnwrap(typeof(Microsoft.VisualStudio.TextTemplating.Engine).Assembly.FullName, typeof(ShadowCopyMonitor).FullName) as ShadowCopyMonitor;
                if ((monitor2 != null) && monitor2.AreShadowCopiesObsolete)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                this.transformDomainUseCount = 0;
                if (this.transformDomain != null)
                {
                    AppDomain.Unload(this.transformDomain);
                    this.transformDomain = null;
                }
            }
            if (this.transformDomain == null)
            {
                AppDomainSetup info = new AppDomainSetup {
                    LoaderOptimization = LoaderOptimization.SingleDomain
                };
                if (GetShadowCopyOption())
                {
                    info.ShadowCopyDirectories = string.Empty;
                    info.ShadowCopyFiles = "true";
                }
                string friendlyName = "throwawayDomain" + Guid.NewGuid().ToString("N");
                this.transformDomain = AppDomain.CreateDomain(friendlyName, null, info);
                this.transformDomainUseCount = 0;
            }
            if (this.transformDomain == null)
            {
                throw new InvalidOperationException(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.AppDomainFailed);
            }
            this.transformDomainUseCount++;
            return this.transformDomain;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (!string.IsNullOrWhiteSpace(assemblyReference))
            {
                assemblyReference = this.ExpandAllVariables(assemblyReference);
                if (Path.IsPathRooted(assemblyReference))
                {
                    Zone zone = Zone.CreateFromUrl(new Uri(assemblyReference).AbsoluteUri);
                    if ((zone.SecurityZone == SecurityZone.Trusted) || (zone.SecurityZone == SecurityZone.MyComputer))
                    {
                        return assemblyReference;
                    }
                    this.LogError(false, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.AssemblyReferenceFailed, new object[] { string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.PathNotTrusted, new object[] { assemblyReference }) }), -1, -1, this.inputFile);
                    return string.Empty;
                }
                string location = GlobalAssemblyCacheHelper.GetLocation(assemblyReference);
                if (!string.IsNullOrEmpty(location))
                {
                    return location;
                }
                OrchestratorPackage singleton = OrchestratorPackage.Singleton;
                if (singleton != null)
                {
                    string vSInstallDir = GetVSInstallDir(singleton.ApplicationRegistryRoot);
                    if (!string.IsNullOrEmpty(vSInstallDir))
                    {
                        location = Path.Combine(Path.Combine(vSInstallDir, "PublicAssemblies"), assemblyReference);
                        if (File.Exists(location))
                        {
                            return location;
                        }
                    }
                }
                IVsExtensionManager service = this.serviceProvider.GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;
                if (service != null)
                {
                    location = this.ResolveAssemblyReferenceViaExtensionManager(assemblyReference, service);
                    if (!string.IsNullOrEmpty(location))
                    {
                        return location;
                    }
                }
            }
            return assemblyReference;
        }

        private string ResolveAssemblyReferenceViaExtensionManager(string assemblyReference, IVsExtensionManager extensionManager)
        {
            IEnumerable<string> source = from path in from e in extensionManager.GetEnabledExtensions() select e.InstallPath
                select Path.Combine(path, assemblyReference) into qualifiedName
                where File.Exists(qualifiedName)
                select qualifiedName;
            switch (source.Count<string>())
            {
                case 0:
                    return null;

                case 1:
                    return source.First<string>();
            }
            string str = source.First<string>();
            this.LogError(true, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.MultipleAssemblyMatch, new object[] { assemblyReference, str }), -1, -1, this.inputFile);
            return str;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            OrchestratorPackage singleton = OrchestratorPackage.Singleton;
            if (singleton != null)
            {
                string name = @"TextTemplating\DirectiveProcessors\" + processorName;
                using (RegistryKey key = singleton.ApplicationRegistryRoot.OpenSubKey(name, false))
                {
                    if (key != null)
                    {
                        List<string> list = new List<string>(key.GetValueNames());
                        string str2 = key.GetValue("Class") as string;
                        if (!string.IsNullOrEmpty(str2))
                        {
                            string str3 = string.Empty;
                            Assembly assembly = null;
                            try
                            {
                                if (list.Contains("Assembly"))
                                {
                                    str3 = key.GetValue("Assembly") as string;
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        assembly = Assembly.Load(str3);
                                    }
                                }
                                else if (list.Contains("CodeBase"))
                                {
                                    str3 = key.GetValue("CodeBase") as string;
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        assembly = Assembly.LoadFrom(str3);
                                    }
                                }
                                if (assembly == null)
                                {
                                    throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TypeResolveFailed, new object[] { processorName }));
                                }
                            }
                            catch (Exception exception)
                            {
                                this.LogError(true, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.DPAssemblyLoadFail, new object[] { str3, processorName, exception.ToString() }), -1, -1, this.inputFile);
                                throw;
                            }
                            if (!assembly.PermissionSet.IsUnrestricted())
                            {
                                this.LogError(true, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.NoFullTrust, new object[] { assembly.FullName, processorName }), -1, -1, this.inputFile);
                                throw new SecurityException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.NoFullTrust, new object[] { assembly.FullName, processorName }));
                            }
                            try
                            {
                                Type type = assembly.GetType(str2);
                                if (type == null)
                                {
                                    throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TypeResolveFailed, new object[] { processorName }));
                                }
                                return type;
                            }
                            catch (Exception exception2)
                            {
                                this.LogError(true, string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.DPTypeLoadFail, new object[] { str2, str3, processorName, exception2.ToString() }), -1, -1, this.inputFile);
                                throw;
                            }
                        }
                    }
                }
            }
            throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TypeResolveFailed, new object[] { processorName }));
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (string.IsNullOrEmpty(directiveId))
            {
                throw new ArgumentNullException("directiveId");
            }
            if (string.IsNullOrEmpty(processorName))
            {
                throw new ArgumentNullException("processorName");
            }
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }
            if (StringComparer.OrdinalIgnoreCase.Compare(parameterName, "namespaceHint") == 0)
            {
                string str = CallContext.LogicalGetData("NamespaceHint") as string;
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return string.Empty;
            }
            if (StringComparer.OrdinalIgnoreCase.Compare(parameterName, "projects") == 0)
            {
                if (this.Dte.Solution == null)
                {
                    return string.Empty;
                }
                Dictionary<Project, string> projects = new Dictionary<Project, string>();
                for (int i = 1; i <= this.Dte.Solution.Projects.Count; i++)
                {
                    ProcessProject(this.Dte.Solution.Projects.Item(i), projects, string.Empty);
                }
                List<string> list = new List<string>();
                foreach (Project project in projects.Keys)
                {
                    list.Add(projects[project]);
                }
                return string.Join("|", list.ToArray());
            }
            if (StringComparer.OrdinalIgnoreCase.Compare(parameterName, "projectDefaultNamespace") == 0)
            {
                if (this.hierarchy != null)
                {
                    object obj2;
                    this.hierarchy.GetProperty(0xfffffffe, -2027, out obj2);
                    Project project2 = obj2 as Project;
                    if (project2 != null)
                    {
                        foreach (Property property in project2.Properties)
                        {
                            if (StringComparer.Ordinal.Compare(property.Name, "DefaultNamespace") == 0)
                            {
                                return property.Value.ToString();
                            }
                        }
                    }
                }
                return string.Empty;
            }
            if (parameterName.Contains(":"))
            {
                return this.ResolveProjectParameter(parameterName);
            }
            return string.Empty;
        }

        public string ResolvePath(string path)
        {
            if (Path.IsPathRooted(path) || string.IsNullOrEmpty(this.inputFile))
            {
                return path;
            }
            string str = Path.Combine(Path.GetDirectoryName(this.inputFile), path);
            if (!File.Exists(str) && !Directory.Exists(str))
            {
                throw new FileNotFoundException(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.UnableToLocateFile, str);
            }
            return str;
        }

        private string ResolveProjectParameter(string parameterName)
        {
            string[] strArray = parameterName.Split(new char[] { ':' });
            if (strArray.Length == 2)
            {
                Dictionary<Project, string> projects = new Dictionary<Project, string>();
                for (int i = 1; i <= this.Dte.Solution.Projects.Count; i++)
                {
                    ProcessProject(this.Dte.Solution.Projects.Item(i), projects, string.Empty);
                }
                foreach (Project project in projects.Keys)
                {
                    Project project2 = null;
                    if (StringComparer.Ordinal.Compare(projects[project], strArray[0]) == 0)
                    {
                        project2 = project;
                    }
                    if ((project2 == null) && (StringComparer.Ordinal.Compare(project.Name, strArray[0]) == 0))
                    {
                        project2 = project;
                    }
                    if (project2 != null)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Compare("OutputPath", strArray[1]) == 0)
                        {
                            if (project2.ConfigurationManager == null)
                            {
                                return string.Empty;
                            }
                            Configuration activeConfiguration = project2.ConfigurationManager.ActiveConfiguration;
                            if (activeConfiguration == null)
                            {
                                return string.Empty;
                            }
                            return Path.Combine(project2.Properties.Item("FullPath").Value.ToString(), activeConfiguration.Properties.Item("OutputPath").Value.ToString());
                        }
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        public void SetFileExtension(string extension)
        {
            if (this.callback != null)
            {
                this.callback.SetFileExtension(extension);
            }
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            if (this.callback != null)
            {
                this.callback.SetOutputEncoding(encoding, fromOutputDirective);
            }
        }

        private void SolutionEvents_AfterClosing()
        {
            if ((this.errorProvider != null) && (this.errorProvider.Tasks != null))
            {
                this.errorProvider.Tasks.Clear();
            }
        }

        private void task_Navigate(object sender, EventArgs e)
        {
            ErrorTask task = sender as ErrorTask;
            if (((task != null) && !string.IsNullOrEmpty(task.Document)) && File.Exists(task.Document))
            {
                IVsUIHierarchy hierarchy;
                uint num;
                IVsWindowFrame frame;
                VsShellUtilities.OpenDocument(this.serviceProvider, task.Document, Guid.Empty, out hierarchy, out num, out frame);
                if (frame != null)
                {
                    task.HierarchyItem = hierarchy;
                    this.errorProvider.Refresh();
                    IVsTextView textView = VsShellUtilities.GetTextView(frame);
                    if (textView != null)
                    {
                        textView.SetSelection(task.Line, task.Column, task.Line, task.Column);
                    }
                }
            }
        }

        internal void UnloadGenerationAppDomain()
        {
            if (this.transformDomain != null)
            {
                AppDomain.Unload(this.transformDomain);
                this.transformDomain = null;
            }
        }

        public ITextTemplatingCallback Callback
        {
            get
            {
                return this.callback;
            }
            set
            {
                this.callback = value;
            }
        }

        private DTE Dte
        {
            get
            {
                if (this.dte == null)
                {
                    try
                    {
                        this.dte = this.package.GetDTE();
                    }
                    catch (InvalidCastException)
                    {
                        throw;
                    }
                }
                return this.dte;
            }
        }

        public ITextTemplatingEngine Engine
        {
            get
            {
                return this.engine;
            }
        }

        public object Hierarchy
        {
            get
            {
                return this.hierarchy;
            }
            set
            {
                this.hierarchy = value as IVsHierarchy;
            }
        }

        public ITextTemplatingEngineHost Host
        {
            get
            {
                return this;
            }
        }

        public string InputFile
        {
            get
            {
                return this.inputFile;
            }
            set
            {
                this.inputFile = value;
            }
        }

        internal bool LastInvocationRaisedErrors
        {
            get
            {
                return this.lastInvocationRaisedErrors;
            }
            set
            {
                this.lastInvocationRaisedErrors = value;
            }
        }

        public ITextTemplatingSession Session
        {
            get
            {
                return this.transformationSession;
            }
            set
            {
                if (this.transformationSessionImplicitlyCreated)
                {
                    this.transformationSessionImplicitlyCreated = false;
                }
                this.transformationSession = value;
            }
        }

        public IList<string> StandardAssemblyReferences
        {
            get
            {
                return new string[] { "System", typeof(DependencyObject).Assembly.Location, base.GetType().Assembly.Location };
            }
        }

        public IList<string> StandardImports
        {
            get
            {
                return new string[] { "System" };
            }
        }

        public string TemplateFile
        {
            get
            {
                return this.inputFile;
            }
        }
    }
}

