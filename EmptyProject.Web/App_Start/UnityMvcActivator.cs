using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EmptyProject.Web.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(EmptyProject.Web.App_Start.UnityWebActivator), "Shutdown")]

namespace EmptyProject.Web.App_Start
{
    public static class UnityWebActivator
    {
      public static void Start()
      {
        var container = UnityConfig.GetConfiguredContainer();

        FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
        FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

        DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
      }

      public static void Shutdown()
      {
        var container = UnityConfig.GetConfiguredContainer();
        container.Dispose();
      }
  }
}