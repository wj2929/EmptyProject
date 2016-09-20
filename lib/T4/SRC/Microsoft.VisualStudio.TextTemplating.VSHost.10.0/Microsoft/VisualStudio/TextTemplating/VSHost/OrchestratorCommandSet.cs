namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.Internal.Performance;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using VSLangProj;
    using VsWebSite;
    using VsWebSite90;

    internal sealed class OrchestratorCommandSet
    {
        private Microsoft.Internal.Performance.CodeMarkers codeMarkers = Microsoft.Internal.Performance.CodeMarkers.Instance;
        private DTE dte;
        private IMenuCommandService menuService;
        private static OutputWindowPane outputPaneStorage;
        private IServiceProvider serviceProvider;

        public OrchestratorCommandSet(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            MenuCommand[] commandArray = new MenuCommand[] { new OleMenuCommand(new EventHandler(this.OnMenuGenerateAllCode), null, new EventHandler(this.OnStatusGenerateAllCode), CommandIds.GenerateAllCode) };
            if (this.MenuService != null)
            {
                for (int i = 0; i < commandArray.Length; i++)
                {
                    this.MenuService.AddCommand(commandArray[i]);
                }
            }
        }

        private void BeginErrorSession()
        {
            Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage orchestratorPackage = this.OrchestratorPackage;
            if (orchestratorPackage != null)
            {
                orchestratorPackage.BeginErrorSession();
            }
        }

        private bool CalculateCommandVisibility()
        {
            IVsSolution service = (IVsSolution) this.serviceProvider.GetService(typeof(SVsSolution));
            if (service != null)
            {
                IEnumHierarchies hierarchies;
                Guid empty = Guid.Empty;
                if (ErrorHandler.Succeeded(service.GetProjectEnum(0x1b, ref empty, out hierarchies)) && (hierarchies != null))
                {
                    uint num2;
                    Guid gUID = typeof(STextTemplating).GUID;
                    IVsHierarchy[] rgelt = new IVsHierarchy[1];
                    while ((hierarchies.Next(1, rgelt, out num2) == 0) && (num2 == 1))
                    {
                        IVsProjectStartupServices startUpServices = GetStartUpServices(rgelt[0]);
                        if ((startUpServices != null) && StartupServicesReferencesService(startUpServices, gUID))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void ClearCustomTextTemplatingToolErrorStatus()
        {
            Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage orchestratorPackage = this.OrchestratorPackage;
            if (orchestratorPackage != null)
            {
                orchestratorPackage.ClearTextTemplatingServiceErrorStatus();
            }
        }

        private void EndErrorSession()
        {
            Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage orchestratorPackage = this.OrchestratorPackage;
            if (orchestratorPackage != null)
            {
                orchestratorPackage.EndErrorSession();
            }
        }

        private void ExecuteCommandIfEnabled(string commandName)
        {
            Commands commands = this.Dte.Commands;
            EnvDTE.Command command = commands.Item(commandName, 0);
            if ((command != null) && command.IsAvailable)
            {
                object customIn = null;
                object customOut = null;
                commands.Raise(command.Guid, command.ID, ref customIn, ref customOut);
            }
        }

        private bool FindCustomTextTemplatingToolErrorStatus()
        {
            bool flag = false;
            Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage orchestratorPackage = this.OrchestratorPackage;
            if (orchestratorPackage != null)
            {
                flag = orchestratorPackage.FindTextTemplatingServiceErrorStatus();
            }
            return flag;
        }

        private void FindProjectItemDependents(ProjectItem item, List<ProjectItem> toProcess)
        {
            VSProjectItem item2 = item.Object as VSProjectItem;
            VSWebProjectItem item3 = item.Object as VSWebProjectItem;
            bool flag = false;
            if ((item2 != null) || (item3 != null))
            {
                string customTool = GetCustomTool(item);
                if ((customTool != null) && ((StringComparer.OrdinalIgnoreCase.Compare(customTool, "TextTemplatingFileGenerator") == 0) || (StringComparer.OrdinalIgnoreCase.Compare(customTool, "TextTemplatingFilePreprocessor") == 0)))
                {
                    flag = true;
                }
            }
            if (item.ProjectItems != null)
            {
                foreach (ProjectItem item4 in item.ProjectItems)
                {
                    this.FindProjectItemDependents(item4, toProcess);
                }
            }
            if (flag)
            {
                toProcess.Add(item);
            }
        }

        private void FindProjectItems(Project project, List<ProjectItem> toProcess)
        {
            if ((((project.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") || (project.Kind == "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}")) || (project.Kind == "{E24C65DC-7377-472b-9ABA-BC803B73C61A}")) && (project.ProjectItems != null))
            {
                foreach (ProjectItem item in project.ProjectItems)
                {
                    this.FindProjectItemDependents(item, toProcess);
                }
            }
            else if ((project.Object is SolutionFolder) && (project.ProjectItems != null))
            {
                foreach (ProjectItem item2 in project.ProjectItems)
                {
                    Project project2 = item2.Object as Project;
                    if (project2 != null)
                    {
                        this.FindProjectItems(project2, toProcess);
                    }
                }
            }
        }

        private static string GetCustomTool(ProjectItem item)
        {
            if (item.Properties != null)
            {
                Property property = null;
                try
                {
                    property = item.Properties.Item("CustomTool");
                }
                catch (ArgumentException)
                {
                }
                if (property != null)
                {
                    string str = property.Value as string;
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
            }
            return null;
        }

        private OutputWindowPane GetPane(string windowName)
        {
            OutputWindow window2 = (OutputWindow) this.Dte.Windows.Item("{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}").Object;
            try
            {
                return window2.OutputWindowPanes.Item(windowName);
            }
            catch (ArgumentException)
            {
                return window2.OutputWindowPanes.Add(windowName);
            }
        }

        internal static IVsProjectStartupServices GetStartUpServices(IVsHierarchy hierarchy)
        {
            object obj2;
            if (ErrorHandler.Succeeded(hierarchy.GetProperty(0xfffffffe, -2040, out obj2)))
            {
                return (obj2 as IVsProjectStartupServices);
            }
            return null;
        }

        private void OnMenuGenerateAllCode(object sender, EventArgs e)
        {
            if (OrchestratorOptionsAutomation.ShowSecurityWarningDialog(this.ServiceProvider))
            {
                this.WriteLine();
                this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TransformationCancelled);
            }
            else
            {
                Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage.ShowSecurityDialogDuringBatchRun = false;
                try
                {
                    if (((this.Dte.Solution != null) && (this.Dte.Solution.Projects != null)) && !this.SaveAllOpenFiles())
                    {
                        Application.DoEvents();
                        this.Dte.ExecuteCommand("View.Output", "");
                        this.outputPane.Clear();
                        this.WriteLine();
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestratorStartCodeGen);
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationSeparator);
                        this.outputPane.Activate();
                        this.BeginErrorSession();
                        Application.DoEvents();
                        List<ProjectItem> toProcess = new List<ProjectItem>();
                        foreach (Project project in this.Dte.Solution.Projects)
                        {
                            this.FindProjectItems(project, toProcess);
                        }
                        bool flag2 = false;
                        IVsStatusbar service = this.ServiceProvider.GetService(typeof(IVsStatusbar)) as IVsStatusbar;
                        uint pdwCookie = 0;
                        uint count = (uint) toProcess.Count;
                        if (service != null)
                        {
                            service.Progress(ref pdwCookie, 1, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TextTemplatingStatusBarLabel, 0, count);
                        }
                        this.codeMarkers.CodeMarker(Microsoft.Internal.Performance.CodeMarkerEvent.perfVSWhitehorseT4CodeGenerationBegin);
                        uint nComplete = 1;
                        foreach (ProjectItem item in toProcess)
                        {
                            Application.DoEvents();
                            if (item != null)
                            {
                                flag2 |= this.RunCustomTool(item);
                            }
                            if (service != null)
                            {
                                service.Progress(ref pdwCookie, 1, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.TextTemplatingStatusBarLabel, nComplete, count);
                            }
                            nComplete += 1;
                            Application.DoEvents();
                        }
                        this.codeMarkers.CodeMarker(Microsoft.Internal.Performance.CodeMarkerEvent.perfVSWhitehorseT4CodeGenerationEnd);
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationSeparator);
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestratorEndCodeGen);
                        this.EndErrorSession();
                        if (service != null)
                        {
                            service.Progress(ref pdwCookie, 0, string.Empty, count, count);
                        }
                        this.ExecuteCommandIfEnabled("View.SolutionExplorer");
                        this.ExecuteCommandIfEnabled("View.Refresh");
                        if (flag2)
                        {
                            this.ExecuteCommandIfEnabled("View.ErrorList");
                        }
                        else
                        {
                            this.ExecuteCommandIfEnabled("View.Output");
                        }
                    }
                }
                finally
                {
                    Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage.ShowSecurityDialogDuringBatchRun = true;
                }
            }
        }

        private void OnStatusGenerateAllCode(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            if (command != null)
            {
                bool flag = this.CalculateCommandVisibility();
                command.Visible = flag;
                bool flag2 = flag;
                if (flag2)
                {
                    int num;
                    IVsSolutionBuildManager service = (IVsSolutionBuildManager) this.serviceProvider.GetService(typeof(SVsSolutionBuildManager));
                    if (((service != null) && ErrorHandler.Succeeded(service.QueryBuildManagerBusy(out num))) && (num == 1))
                    {
                        flag2 = false;
                    }
                }
                command.Enabled = flag2;
            }
        }

        private bool RunCustomTool(ProjectItem item)
        {
            string customTool = GetCustomTool(item);
            VSProjectItem item2 = item.Object as VSProjectItem;
            VSWebProjectItem2 item3 = item.Object as VSWebProjectItem2;
            bool flag = true;
            bool flag2 = false;
            try
            {
                string text1 = (string) item.Properties.Item("FullPath").Value;
            }
            catch (ArgumentException)
            {
                flag = false;
                this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationSkippingNoPath, new object[] { item.Name });
            }
            if ((flag && (item.ProjectItems != null)) && (item.ProjectItems.Count > 0))
            {
                foreach (ProjectItem item4 in item.ProjectItems)
                {
                    string str2 = "";
                    try
                    {
                        str2 = (string) item4.Properties.Item("FullPath").Value;
                    }
                    catch (ArgumentException)
                    {
                        flag = false;
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationSkippingNoPath, new object[] { item4.Name });
                    }
                    if ((!string.IsNullOrEmpty(str2) && !this.Dte.SourceControl.IsItemUnderSCC(str2)) && (File.Exists(str2) && ((File.GetAttributes(str2) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)))
                    {
                        flag = false;
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationSkippingReadOnly, new object[] { str2 });
                    }
                }
            }
            if (flag)
            {
                this.Write(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestratorRunLine, new object[] { customTool, item.Name });
                try
                {
                    this.ClearCustomTextTemplatingToolErrorStatus();
                    if (item2 != null)
                    {
                        item2.RunCustomTool();
                    }
                    else if (item3 != null)
                    {
                        item3.RunCustomTool();
                    }
                    flag2 = this.FindCustomTextTemplatingToolErrorStatus();
                    if (flag2)
                    {
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationLineFailed);
                    }
                    else
                    {
                        this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationLineSucceeded);
                    }
                }
                catch (Exception exception)
                {
                    if (CriticalException.IsCriticalException(exception))
                    {
                        throw;
                    }
                    this.WriteLine(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OrchestrationLineFailed);
                    flag2 = true;
                }
            }
            this.outputPane.Activate();
            return flag2;
        }

        private bool SaveAllOpenFiles()
        {
            try
            {
                IVsSolutionBuildManager2 service = this.ServiceProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
                if (service != null)
                {
                    ErrorHandler.ThrowOnFailure(service.SaveDocumentsBeforeBuild(null, 0xfffffffe, 0));
                }
            }
            catch (COMException exception)
            {
                if ((exception.ErrorCode != -2147467260) && (exception.ErrorCode != -2147221492))
                {
                    throw;
                }
                return true;
            }
            return false;
        }

        internal static bool StartupServicesReferencesService(IVsProjectStartupServices startup, Guid serviceId)
        {
            int num2;
            IEnumProjectStartupServices ppenum = null;
            ErrorHandler.ThrowOnFailure(startup.GetStartupServiceEnum(out ppenum));
            uint pceltFetched = 0;
            Guid[] rgelt = new Guid[1];
            do
            {
                num2 = ppenum.Next(1, rgelt, out pceltFetched);
                ErrorHandler.ThrowOnFailure(num2);
                if ((pceltFetched == 1) && (rgelt[0].CompareTo(serviceId) == 0))
                {
                    return true;
                }
            }
            while (num2 != 1);
            return false;
        }

        private void Write(string text)
        {
            this.outputPane.OutputString(text);
        }

        private void Write(string format, params object[] args)
        {
            this.outputPane.OutputString(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        private void WriteLine()
        {
            this.outputPane.OutputString(Environment.NewLine);
        }

        private void WriteLine(string text)
        {
            this.Write(text + Environment.NewLine);
        }

        private void WriteLine(string format, params object[] args)
        {
            this.Write(format + Environment.NewLine, args);
        }

        private DTE Dte
        {
            get
            {
                if (this.dte == null)
                {
                    try
                    {
                        this.dte = (DTE) this.serviceProvider.GetService(typeof(DTE));
                    }
                    catch (InvalidCastException)
                    {
                        throw;
                    }
                }
                return this.dte;
            }
        }

        private IMenuCommandService MenuService
        {
            get
            {
                if (this.menuService == null)
                {
                    try
                    {
                        this.menuService = (IMenuCommandService) this.serviceProvider.GetService(typeof(IMenuCommandService));
                    }
                    catch (InvalidCastException)
                    {
                        throw;
                    }
                }
                return this.menuService;
            }
        }

        private Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage OrchestratorPackage
        {
            get
            {
                return (this.ServiceProvider.GetService(typeof(Package)) as Microsoft.VisualStudio.TextTemplating.VSHost.OrchestratorPackage);
            }
        }

        private OutputWindowPane outputPane
        {
            get
            {
                if (outputPaneStorage == null)
                {
                    outputPaneStorage = this.GetPane(Microsoft.VisualStudio.TextTemplating.VSHost.Resources.OutputPaneName);
                }
                return outputPaneStorage;
            }
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }
    }
}

