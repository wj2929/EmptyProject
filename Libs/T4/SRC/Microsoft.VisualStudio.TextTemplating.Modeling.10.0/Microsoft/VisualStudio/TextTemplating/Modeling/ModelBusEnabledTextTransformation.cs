namespace Microsoft.VisualStudio.TextTemplating.Modeling
{
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Modeling.Integration;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.TextTemplating;
    using Microsoft.VisualStudio.TextTemplating.VSHost;
    using System;
    using System.Reflection;

    public abstract class ModelBusEnabledTextTransformation : ModelingTextTransformation
    {
        private static CompositionConfigurationManager configuration;
        private static Microsoft.VisualStudio.Modeling.Integration.ModelBus modelBus;
        private ITextTemplatingEngineHost reflectedHost;
        private static bool serviceProviderInitialized;

        protected ModelBusEnabledTextTransformation()
        {
        }

        private void CurrentDomain_Cleanup(object sender, EventArgs args)
        {
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(this.CurrentDomain_Cleanup);
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(this.CurrentDomain_Cleanup);
            if (CompositionServices.ServiceContainer != null)
            {
                CompositionServices.ServiceContainer.RemoveService(typeof(SModelBus));
                CompositionServices.ServiceContainer.RemoveService(typeof(IModelBus));
            }
            if ((modelBus != null) && !modelBus.Disposed)
            {
                modelBus.Dispose();
                modelBus = null;
            }
            configuration = null;
        }

        protected override void OnSessionChanged(ITextTemplatingSession oldSession, ITextTemplatingSession newSession)
        {
            if (modelBus != null)
            {
                foreach (IStatefulAdapterManager manager in modelBus.GetRegisteredAdapterManagers<IStatefulAdapterManager>())
                {
                    manager.ClearState();
                }
            }
        }

        protected virtual void ReportError(ErrorCategory category, string message)
        {
            if (category == ErrorCategory.Error)
            {
                base.Error(message);
            }
            else
            {
                base.Warning(message);
            }
        }

        protected IModelBus ModelBus
        {
            get
            {
                return (this.ServiceProvider.GetService(typeof(SModelBus)) as IModelBus);
            }
        }

        private ITextTemplatingEngineHost ReflectedHost
        {
            get
            {
                if (this.reflectedHost == null)
                {
                    try
                    {
                        PropertyInfo property = base.GetType().GetProperty("Host", BindingFlags.Public | BindingFlags.Instance);
                        if (property != null)
                        {
                            this.reflectedHost = property.GetValue(this, new object[0]) as ITextTemplatingEngineHost;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (Microsoft.VisualStudio.Modeling.CriticalException.IsCriticalException(exception))
                        {
                            throw;
                        }
                    }
                }
                return this.reflectedHost;
            }
        }

        protected override System.IServiceProvider ServiceProvider
        {
            get
            {
                if (!serviceProviderInitialized)
                {
                    ITextTemplatingEngineHost reflectedHost = this.ReflectedHost;
                    if (reflectedHost == null)
                    {
                        base.Error("T4 ModelBus services are unavailable unless template sets hostSpecific=true.");
                        return null;
                    }
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleProvider = reflectedHost as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    if (oleProvider == null)
                    {
                        base.Error("T4 Modelbus services are unavailable unless host implements Microsoft.VisualStudio.OLE.Interop.IServiceProvider.");
                        return null;
                    }
                    CompositionServices.InitializeServices(oleProvider);
                    IComponentModel service = CompositionServices.ServiceProvider.GetService(typeof(SComponentModel)) as IComponentModel;
                    configuration = new CompositionConfigurationManager(service.DefaultExportProvider, "T4VSHost");
                    modelBus = new Microsoft.VisualStudio.Modeling.Integration.ModelBus(CompositionServices.ServiceProvider, null, configuration);
                    CompositionServices.ServiceContainer.AddService(typeof(SModelBus), modelBus);
                    CompositionServices.ServiceContainer.AddService(typeof(IModelBus), modelBus);
                    AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.CurrentDomain_Cleanup);
                    AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.CurrentDomain_Cleanup);
                    serviceProviderInitialized = true;
                }
                modelBus.ErrorCallback = new Action<ErrorCategory, string>(this.ReportError);
                return CompositionServices.ServiceProvider;
            }
        }
    }
}

