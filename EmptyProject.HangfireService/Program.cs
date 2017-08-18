using Hangfire;
using Hangfire.SqlServer;
using log4net.Config;
using EmptyProject.HangfireService.Code;
using EmptyProject.HangfireService.Code.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace EmptyProject.HangfireService
{
    class Program
    {
        static void Main(string[] args)
        {
            var logCfg = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"log4net.config"));
            XmlConfigurator.ConfigureAndWatch(logCfg);

            HostFactory.Run(x =>
            {
                x.Service<MyService>(s =>
                {
                    s.ConstructUsing(name => new MyService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription(GlobalController.ConfigAccessHelper.ConfigEntity.ServiceDescription);
                x.SetDisplayName(GlobalController.ConfigAccessHelper.ConfigEntity.DisplayServiceName);
                x.SetServiceName(GlobalController.ConfigAccessHelper.ConfigEntity.ServiceName);
            });
        }
    }
}
