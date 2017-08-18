using Hangfire;
using Hangfire.Logging;
//using log4net;
//using log4net.Config;
using Microsoft.Owin.Hosting;
using EmptyProject.HangfireService.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace EmptyProject.HangfireService
{
    public class MyService
    {
        private static readonly ILog _logger = LogProvider.For<MyService>();
        private IDisposable _host;
        public MyService()
        {
        }

        public void Start() 
        {
            try
            {
                this._host = WebApp.Start<Startup>(GlobalController.ConfigAccessHelper.ConfigEntity.EndPoint);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Topshelf starting occured errors.", ex);
            }
        }

        public void Stop()
        {
            try
            {
                this._host.Dispose();
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Topshelf starting occured errors.", ex);
            }
        }
    }

}
