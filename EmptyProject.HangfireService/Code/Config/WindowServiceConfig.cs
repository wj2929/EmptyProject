using BC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.HangfireService.Code.Config
{
    public class WindowServiceConfig : IConfigBase<WindowServiceConfig>
    {
        // Methods
        public WindowServiceConfig FromConfig(string Config)
        {
            if (Config.IsEmpty())
            {
                return new WindowServiceConfig();
            }
            return new WindowServiceConfig { ServiceName = Config.GetTag("ServiceName"), DisplayServiceName = Config.GetTag("DisplayServiceName"), ServiceDescription = Config.GetTag("ServiceDescription"), EndPoint = Config.GetTag("EndPoint") };
        }

        public string ToConfig()
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("<WindowServiceConfig>");
            builder1.Append("<ServiceName>");
            builder1.Append(this.ServiceName);
            builder1.Append("</ServiceName>");
            builder1.Append("<DisplayServiceName>");
            builder1.Append(this.DisplayServiceName);
            builder1.Append("</DisplayServiceName>");
            builder1.Append("<ServiceDescription>");
            builder1.Append(this.ServiceDescription);
            builder1.Append("</ServiceDescription>");
            builder1.Append("<EndPoint>");
            builder1.Append(this.EndPoint);
            builder1.Append("</EndPoint>");
            builder1.Append("</WindowServiceConfig>");
            return builder1.ToString();
        }

        // Properties
        public string DisplayServiceName { get; set; }

        public string EndPoint { get; set; }

        public string ServiceDescription { get; set; }

        public string ServiceName { get; set; }

    }
}
