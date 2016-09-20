namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.ExtensibilityHosting;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;

    internal class LocalComponentModel : SComponentModel, IComponentModel
    {
        private VsCatalogProvider catalogProvider;
        private CompositionContainer container;

        internal LocalComponentModel(VsCatalogProvider catalogProvider, CompositionContainer container)
        {
            this.catalogProvider = catalogProvider;
            this.container = container;
        }

        public ComposablePartCatalog GetCatalog(string catalogName)
        {
            return this.catalogProvider.GetCatalog(catalogName);
        }

        public IEnumerable<T> GetExtensions<T>() where T: class
        {
            return this.container.GetExportedValues<T>();
        }

        public T GetService<T>() where T: class
        {
            return this.container.GetExportedValue<T>();
        }

        public ComposablePartCatalog DefaultCatalog
        {
            get
            {
                return this.GetCatalog("Microsoft.VisualStudio.Default");
            }
        }

        public ICompositionService DefaultCompositionService
        {
            get
            {
                return this.container;
            }
        }

        public ExportProvider DefaultExportProvider
        {
            get
            {
                return this.container;
            }
        }
    }
}

