using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
//using BasicCommon.Schedule;
using Microsoft.Owin;
using Owin;
using Topshelf;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Microsoft.Owin.Diagnostics;
using Hangfire.RecurringJobExtensions;
using EmptyProject.HangfireService.Code;

[assembly: OwinStartup(typeof(EmptyProject.HangfireService.Startup))]
namespace EmptyProject.HangfireService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //HostFactory.Run(x =>
            //    {
                    //GlobalConfiguration.Configuration
                    //    // Use connection string name defined in `web.config` or `app.config`
                    //    .UseSqlServerStorage("HangfireConnection");

                    //app.UseHangfireDashboard();
                    //app.UseHangfireServer();

                    app.UseErrorPage();
                    app.UseWelcomePage("/");

                    GlobalConfiguration.Configuration.UseSqlServerStorage(
                        "HangfireConnection",
                        new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });

                    IContainer container = app.UseAutofac();

                    CronJob.AddOrUpdate("Config/RecurringJob.json");

                    //BackgroundJob.Enqueue(() => new ActivityEndRecurringJob().Test());

                    app.UseHangfireDashboard();


                    //var queues = new[] { "ndlmsdataservice" };
                    app.UseHangfireServer(new BackgroundJobServerOptions
                    {
                        //wait all jobs performed when BackgroundJobServer shutdown.
                        ShutdownTimeout = TimeSpan.FromMinutes(30),
                        //Queues = queues,
                        WorkerCount = Math.Max(Environment.ProcessorCount, 20)
                    });
                //});
        }
    }
}
