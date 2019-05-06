using BC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain.Values.Configs
{
    public class CategoryTypeConfig : IConfigBase<CategoryTypeConfig>
    {
        private IList<CategoryTypeItemConfig> _CategoryTypeItems;
        /// <summary>
        /// 
        /// </summary>
        public IList<CategoryTypeItemConfig> CategoryTypeItems
        {
            get
            {
                if (this._CategoryTypeItems == null)
                    this._CategoryTypeItems = new List<CategoryTypeItemConfig>();

                return this._CategoryTypeItems;
            }
        }

        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<CategoryTypeConfig>");
            sb.Append("<Items>");
            foreach (var Item in this.CategoryTypeItems)
            {
                sb.Append(Item.ToConfig());
            }
            sb.Append("</Items>");
            sb.Append("</CategoryTypeConfig>");

            return sb.ToString();
        }

        /// <summary>
        /// 从配置文件构件对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public CategoryTypeConfig FromConfig(string Config)
        {
            CategoryTypeConfig rInfo = new CategoryTypeConfig();

            if (Config.IsEmpty())
                return rInfo;

            IList<string> _Tags = Config.GetTag("Items").GetTags("Item");

            foreach (string Item in _Tags)
                rInfo.CategoryTypeItems.Add(new CategoryTypeItemConfig().FromConfig(Item.GetTag("Item")));

            return rInfo;
        }

    }

    public class CategoryTypeItemConfig : IConfigBase<CategoryTypeItemConfig>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string Keycode { get; set; }

        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Item>");
            sb.Append("<Name>");
            sb.Append(Name);
            sb.Append("</Name>");
            sb.Append("<Keycode>");
            sb.Append(Keycode);
            sb.Append("</Keycode>");
            sb.Append("</Item>");
            return sb.ToString();
        }

        public CategoryTypeItemConfig FromConfig(string Config)
        {
            CategoryTypeItemConfig rInfo = new CategoryTypeItemConfig();
            if (Config.IsEmpty())
                return rInfo;

            rInfo.Name = Config.GetTag("Name").Trim();
            rInfo.Keycode = Config.GetTag("Keycode").Trim();
            return rInfo;

        }

    }
}
