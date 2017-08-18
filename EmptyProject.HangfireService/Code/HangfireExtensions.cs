using Autofac;
using Autofac.Configuration;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.HangfireService.Code
{
    public static class HangfireExtensions
    {
        public static IContainer UseAutofac(this IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            var assembly = typeof(Startup).Assembly;

            builder.RegisterAssemblyModules(assembly);

            var container = builder.Build();

            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            return container;
        }

        //public static IContainer UseAutofac(this IAppBuilder app)
        //{
        //    var builder = new ContainerBuilder();
        //    var config = new ConfigurationBuilder();
        //    config.AddJsonFile("Autofac.json");
        //    builder.RegisterModule(new ConfigurationModule(config.Build()));
        //    IContainer container = builder.Build();
        //    GlobalConfiguration.Configuration.UseAutofacActivator(container, false);

        //    return container;
        //}

    }
}
