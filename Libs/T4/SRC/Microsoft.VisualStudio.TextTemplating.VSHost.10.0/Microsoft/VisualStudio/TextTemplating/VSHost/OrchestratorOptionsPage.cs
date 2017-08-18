namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Runtime.InteropServices;

    [CLSCompliant(false), Guid("64998AAF-06EE-4517-9D19-A4787DB08F4B")]
    public class OrchestratorOptionsPage : DialogPage
    {
        public OrchestratorOptionsPage()
        {
            base.SettingsRegistryPath = "DSLTools";
        }

        public override object AutomationObject
        {
            get
            {
                return OrchestratorOptionsAutomation.instance;
            }
        }
    }
}

