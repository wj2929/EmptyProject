using BC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain.Values.Configs
{
    public class CategoryConfig : IConfigBase<CategoryConfig>
    {
        private IList<CategoryItemConfig> _CategoryItems;
        /// <summary>
        /// 
        /// </summary>
        public IList<CategoryItemConfig> CategoryItems
        {
            get
            {
                if (this._CategoryItems == null)
                    this._CategoryItems = new List<CategoryItemConfig>();

                return this._CategoryItems;
            }
        }

        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<CategoryConfig>");
            sb.Append("<Items>");
            foreach (var Item in this.CategoryItems)
            {
                sb.Append(Item.ToConfig());
            }
            sb.Append("</Items>");
            sb.Append("</CategoryConfig>");

            return sb.ToString();
        }

        /// <summary>
        /// 从配置文件构件对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public CategoryConfig FromConfig(string Config)
        {
            CategoryConfig rInfo = new CategoryConfig();

            if (Config.IsEmpty())
                return rInfo;

            IList<string> _Tags = Config.GetTag("Items").GetTags("Item");

            foreach (string Item in _Tags)
                rInfo.CategoryItems.Add(new CategoryItemConfig().FromConfig(Item.GetTag("Item")));

            return rInfo;
        }

    }

    public class CategoryItemConfig : IConfigBase<CategoryItemConfig>
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Index { get; set; }
        public int OrderBy { get; set; }
        public string Type { get; set; }
        public string ExtendedConfig { get; set; }
        public string Flag { get; set; }
        public Guid? ParentCategory_Id { get; set; }

        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Item>");
            sb.Append("<Name>");
            sb.Append(Name);
            sb.Append("</Name>");
            sb.Append("<Level>");
            sb.Append(Level);
            sb.Append("</Level>");
            sb.Append("<Index>");
            sb.Append(Index);
            sb.Append("</Index>");
            sb.Append("<OrderBy>");
            sb.Append(OrderBy);
            sb.Append("</OrderBy>");
            sb.Append("<Type>");
            sb.Append(Type);
            sb.Append("</Type>");
            sb.Append("<ExtendedConfig>");
            sb.Append(ExtendedConfig);
            sb.Append("</ExtendedConfig>");
            sb.Append("<Flag>");
            sb.Append(Flag);
            sb.Append("</Flag>");
            sb.Append("<ParentCategory_Id>");
            sb.Append(ParentCategory_Id);
            sb.Append("</ParentCategory_Id>");
            sb.Append("</Item>");
            return sb.ToString();
        }

        public CategoryItemConfig FromConfig(string Config)
        {
            CategoryItemConfig rInfo = new CategoryItemConfig();
            if (Config.IsEmpty())
                return rInfo;

            rInfo.Name = Config.GetTag("Name").Trim();
            rInfo.Level = int.Parse(Config.GetTag("Level").Trim());
            rInfo.Index = Config.GetTag("Index").Trim();
            rInfo.OrderBy = int.Parse(Config.GetTag("OrderBy").Trim());
            rInfo.Type = Config.GetTag("Type").Trim();
            rInfo.ExtendedConfig = Config.GetTag("ExtendedConfig").Trim();
            rInfo.Flag = Config.GetTag("Flag").Trim();
            if(Config.GetTag("ParentCategory_Id").Trim() != string.Empty)
                rInfo.ParentCategory_Id  = Guid.Parse(Config.GetTag("ParentCategory_Id").Trim()) ;
            return rInfo;

        }

    }
}
