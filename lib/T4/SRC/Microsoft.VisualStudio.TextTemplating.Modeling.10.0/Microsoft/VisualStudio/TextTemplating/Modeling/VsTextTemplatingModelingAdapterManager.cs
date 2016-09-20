namespace Microsoft.VisualStudio.TextTemplating.Modeling
{
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Modeling.Integration;
    using Microsoft.VisualStudio.Modeling.Integration.Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    [CLSCompliant(false)]
    public abstract class VsTextTemplatingModelingAdapterManager : VsModelingAdapterManager, IStatefulAdapterManager
    {
        private static List<ModelBusAdapter> disposeList = new List<ModelBusAdapter>();
        public const string HostName = "T4VSHost";

        static VsTextTemplatingModelingAdapterManager()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(VsTextTemplatingModelingAdapterManager.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(VsTextTemplatingModelingAdapterManager.CurrentDomain_Cleanup);
        }

        protected VsTextTemplatingModelingAdapterManager()
        {
        }

        [Conditional("DEBUG")]
        private static void AssertPartitionAdapterReferenceMatch(ModelingAdapterReference adapterReference, Partition partition)
        {
            object alternateId = partition.AlternateId;
        }

        public void ClearState()
        {
            DisposeAdapters();
            SessionStoreCache.DisposeSessionStoreMap();
        }

        protected virtual ModelingDocumentHandler CreateDocumentHandler(ModelingAdapterReference adapterReference, IServiceProvider serviceProvider)
        {
            string adapterReferenceStoreKey = this.GetAdapterReferenceStoreKey(adapterReference);
            Store sessionStore = SessionStoreCache.GetSessionStore(adapterReferenceStoreKey);
            ModelElement root = null;
            if (sessionStore != null)
            {
                root = this.FindStoreRoot(sessionStore, adapterReference, serviceProvider);
            }
            if (root == null)
            {
                root = this.CreateLoadStore(sessionStore, adapterReference, serviceProvider);
                if (root == null)
                {
                    throw new AdapterCreationException(Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.GetErrorLoadFailed(adapterReference.AbsoluteTargetPath));
                }
                SessionStoreCache.CacheSessionStore(adapterReferenceStoreKey, root.Store);
            }
            if (root != null)
            {
                return new VsTextTemplatingModelingDocumentHandler(adapterReference.AbsoluteTargetPath, root);
            }
            return null;
        }

        protected virtual ModelElement CreateLoadStore(Store store, ModelingAdapterReference adapterReference, IServiceProvider serviceProvider)
        {
            ModelElement element = null;
            if (store == null)
            {
                store = new Store(serviceProvider, new Type[0]);
            }
            element = this.LoadPartition(store.DefaultPartition, adapterReference.AbsoluteTargetPath);
            if (element != null)
            {
                element.Partition.AlternateId = new Tuple<string, ModelElement>(adapterReference.AbsoluteTargetPath, element);
            }
            return element;
        }

        protected virtual ISerializerLocator CreateSerializerLocator()
        {
            return new StandardSerializerLocator(this.ExportProvider);
        }

        private static void CurrentDomain_Cleanup(object sender, EventArgs args)
        {
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(VsTextTemplatingModelingAdapterManager.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(VsTextTemplatingModelingAdapterManager.CurrentDomain_Cleanup);
            DisposeAdapters();
        }

        private static void DisposeAdapters()
        {
            foreach (ModelBusAdapter adapter in disposeList)
            {
                if (!adapter.Disposed)
                {
                    adapter.Dispose();
                }
            }
            disposeList.Clear();
        }

        protected override ModelBusAdapter DoCreateAdapter(ModelBusReference reference, IServiceProvider serviceProvider)
        {
            this.CheckCanCreateAdapter(reference);
            ModelingAdapter item = null;
            ModelingDocumentHandler documentHandler = null;
            ModelingAdapterReference adapterReference = reference.AdapterReference as ModelingAdapterReference;
            try
            {
                documentHandler = this.CreateDocumentHandler(adapterReference, base.ModelBus);
                if (documentHandler == null)
                {
                    throw new AdapterCreationException(Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.UnknownErrorOccurred);
                }
                item = this.CreateModelingAdapterInstance(reference, documentHandler.Root);
                if (item == null)
                {
                    throw new AdapterCreationException(Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.UnknownErrorOccurred);
                }
                if (!item.TrySetDocumentHandler(documentHandler))
                {
                    throw new AdapterCreationException(Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.UnknownErrorOccurred);
                }
            }
            catch (Exception)
            {
                if (documentHandler != null)
                {
                    documentHandler.Dispose();
                }
                throw;
            }
            disposeList.Add(item);
            return item;
        }

        protected virtual ModelElement FindStoreRoot(Store store, ModelingAdapterReference adapterReference, IServiceProvider serviceProvider)
        {
            ModelElement element = null;
            Tuple<string, ModelElement> alternateId = store.DefaultPartition.AlternateId as Tuple<string, ModelElement>;
            if (alternateId != null)
            {
                element = alternateId.Item2;
            }
            return element;
        }

        private static ErrorCategory FromKind(SerializationMessageKind kind)
        {
            switch (kind)
            {
                case SerializationMessageKind.Info:
                    return ErrorCategory.Message;

                case SerializationMessageKind.Warning:
                    return ErrorCategory.Warning;

                case SerializationMessageKind.Error:
                    return ErrorCategory.Error;
            }
            return ErrorCategory.Message;
        }

        protected virtual string GetAdapterReferenceStoreKey(ModelingAdapterReference adapterReference)
        {
            return adapterReference.AbsoluteTargetPath;
        }

        protected override ModelBusView GetView(ModelBusAdapter viewOwner, ModelBusReference viewReference)
        {
            throw new NotSupportedException();
        }

        private ModelElement LoadPartition(Partition partition, string modelFile)
        {
            if (string.IsNullOrEmpty(modelFile) || !File.Exists(modelFile))
            {
                base.ModelBus.LogError(ErrorCategory.Error, Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.GetInvalidModelFilePath(modelFile));
                return null;
            }
            ISerializerLocator serializerLocator = this.CreateSerializerLocator();
            if (serializerLocator == null)
            {
                base.ModelBus.LogError(ErrorCategory.Error, Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.GetErrorLocateSerializer(modelFile));
                return null;
            }
            IDomainModelSerializer serializerFromFileName = serializerLocator.GetSerializerFromFileName(modelFile);
            if (serializerFromFileName == null)
            {
                base.ModelBus.LogError(ErrorCategory.Error, Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.GetErrorLocateSerializer(modelFile));
                return null;
            }
            Type domainModelType = serializerFromFileName.DomainModelType;
            if (!(from i in partition.DomainDataDirectory.DomainModels select i.ImplementationType).Any<Type>(t => object.Equals(t, domainModelType)))
            {
                partition.Store.LoadDomainModels(new Type[] { domainModelType });
            }
            SerializationResult serializationResult = new SerializationResult();
            ModelElement element = null;
            using (Transaction transaction = partition.Store.TransactionManager.BeginTransaction("Load model", true))
            {
                element = serializerFromFileName.LoadModel(serializationResult, partition, modelFile, serializerLocator);
                if (!serializationResult.Failed)
                {
                    transaction.Commit();
                }
            }
            if (!serializationResult.Failed && (element != null))
            {
                return element;
            }
            base.ModelBus.LogError(ErrorCategory.Error, Microsoft.VisualStudio.TextTemplating.Modeling.ModelBusExceptionMessages.GetErrorLoadFailed(modelFile));
            if (serializationResult.Failed)
            {
                foreach (SerializationMessage message in serializationResult)
                {
                    base.ModelBus.LogError(FromKind(message.Kind), message.Message);
                }
            }
            return null;
        }

        private System.ComponentModel.Composition.Hosting.ExportProvider ExportProvider
        {
            get
            {
                IComponentModel service = base.ModelBus.GetService(typeof(SComponentModel)) as IComponentModel;
                return service.DefaultExportProvider;
            }
        }
    }
}

