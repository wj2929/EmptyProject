namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.CommonIDE.ExtensibilityHosting;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.ExtensibilityHosting;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.ComponentModel.Design;

    public static class CompositionServices
    {
        private static VsCatalogProvider catalogProvider;
        private static VsShellComponentModelHost componentHost;
        private static CompositionContainer composition;
        private static System.ComponentModel.Design.ServiceContainer container;
        private static ComposablePartCatalog globalCatalog;
        private static LocalComponentModel localComponentModel;
        private static Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider;
        private static bool servicesInitialized = false;

        private static void CleanupFields()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
            if (serviceProvider != null)
            {
                serviceProvider.Dispose();
                serviceProvider = null;
            }
            if (composition != null)
            {
                composition.Dispose();
                composition = null;
            }
            if (globalCatalog != null)
            {
                globalCatalog.Dispose();
                globalCatalog = null;
            }
            componentHost = null;
            catalogProvider = null;
            localComponentModel = null;
        }

        private static void CurrentDomain_Cleanup(object sender, EventArgs args)
        {
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(Microsoft.VisualStudio.TextTemplating.VSHost.CompositionServices.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(Microsoft.VisualStudio.TextTemplating.VSHost.CompositionServices.CurrentDomain_Cleanup);
            CleanupFields();
        }

        public static void InitializeServices(Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleProvider)
        {
            if (oleProvider == null)
            {
                throw new ArgumentNullException("oleProvider");
            }
            if (!servicesInitialized)
            {
                try
                {
                    serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(oleProvider);
                    container = new System.ComponentModel.Design.ServiceContainer(serviceProvider);
                    componentHost = new VsShellComponentModelHost(oleProvider);
                    catalogProvider = new VsCatalogProvider(componentHost);
                    globalCatalog = catalogProvider.GetCatalog("Microsoft.VisualStudio.Default");
                    if (globalCatalog == null)
                    {
                        throw new InvalidOperationException("T4 Composition services initialization error: Failed to get default composition catalog.");
                    }
                    composition = VsCompositionContainer.Create(globalCatalog);
                    composition.ComposeExportedValue<SVsServiceProvider>(new VsServiceProviderWrapper(container));
                    localComponentModel = new LocalComponentModel(catalogProvider, composition);
                    container.AddService(typeof(SComponentModel), localComponentModel);
                    container.AddService(typeof(IComponentModel), localComponentModel);
                    AppDomain.CurrentDomain.ProcessExit += new EventHandler(Microsoft.VisualStudio.TextTemplating.VSHost.CompositionServices.CurrentDomain_Cleanup);
                    AppDomain.CurrentDomain.DomainUnload += new EventHandler(Microsoft.VisualStudio.TextTemplating.VSHost.CompositionServices.CurrentDomain_Cleanup);
                    servicesInitialized = true;
                }
                catch (Exception)
                {
                    CleanupFields();
                    throw;
                }
            }
        }

        public static IServiceContainer ServiceContainer
        {
            get
            {
                return container;
            }
        }

        public static System.IServiceProvider ServiceProvider
        {
            get
            {
                return container;
            }
        }
    }
}

