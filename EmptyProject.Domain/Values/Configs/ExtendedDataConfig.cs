using BC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain.Values.Configs
{
    public class ExtendedDataConfig : AutoBaseConfig<ExtendedDataConfig>
    {
        public ExtendedDataConfig()
        {
            this.Items = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Items { get; set; }
    }
}
