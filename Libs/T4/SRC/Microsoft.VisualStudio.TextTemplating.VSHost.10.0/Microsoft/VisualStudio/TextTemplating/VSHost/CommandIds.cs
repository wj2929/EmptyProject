namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.ComponentModel.Design;

    public static class CommandIds
    {
        private const int cmdIdGenerateAllCode = 0x2020;
        public static readonly CommandID GenerateAllCode = new CommandID(GuidOrchestratorCommandSet, 0x2020);
        public static readonly Guid GuidOrchestratorCommandSet = new Guid("CCD03FEB-6B80-4cdb-AB3A-04702F6E7553");
        public static readonly Guid GuidOrchestratorMenus = new Guid("318676E3-4A0D-4397-8FD0-78D0177D90BC");
    }
}

