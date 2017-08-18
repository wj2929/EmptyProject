using BC.Core;
using EmptyProject.HangfireService.Code.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.HangfireService.Code
{
    public class GlobalController
    {
        public static readonly IConfigAccessHelper<WindowServiceConfig> ConfigAccessHelper = new LocalConfigurationAccessHelper<WindowServiceConfig>("WindowServiceConfig");

    }
}
