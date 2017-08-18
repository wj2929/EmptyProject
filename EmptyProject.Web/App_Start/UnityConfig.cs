using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using BC.DDD.IoC;
namespace EmptyProject.Web.App_Start
{

    public class UnityConfig
    {
      #region Unity Container
      private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
      {
        RegisterTypes(IoCManage.Container);
        return IoCManage.Container;
      });

      /// <summary>
      /// Gets the configured Unity container.
      /// </summary>
      public static IUnityContainer GetConfiguredContainer()
      {
        return container.Value;
      }
      #endregion

      /// <summary>
      /// ע������
      /// </summary>
      /// <param name="container"></param>
      public static void RegisterTypes(IUnityContainer container)
      {

      }
  }
}
