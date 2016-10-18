using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
namespace EmptyProject.Manage.IoC
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        public UnityDependencyResolver(IUnityContainer container)
        {
            this.container = container;
        }

        protected IUnityContainer container;

        #region IDependencyResolver 成员
        /// <summary>
        /// 解析支持任意对象创建的一次注册的服务。
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return (serviceType.IsClass && !serviceType.IsAbstract) ||
                        container.IsRegistered(serviceType) ?
                        container.Resolve(serviceType) : null;

        }
        /// <summary>
        /// 解析多次注册的服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (serviceType.IsClass && !serviceType.IsAbstract) ||
                       container.IsRegistered(serviceType) ?
                       container.ResolveAll(serviceType) : new object[] { };
        }
        #endregion
    }
}