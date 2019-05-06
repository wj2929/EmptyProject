using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using BC.Core;

namespace EmptyProject.Domain.Values.Configs
{
    public class RegularExpressionValidatorConfig : IConfigBase<RegularExpressionValidatorConfig>
    {
        private IDictionary<string, string> _Validators;
        /// <summary>
        /// 规则列表
        /// </summary>
        public IDictionary<string, string> Validators
        {
            get
            {
                if (this._Validators == null)
                    this._Validators = new Dictionary<string, string>();

                return this._Validators;
            }
        }

        #region IConfigBase<RegularExpressionValidatorConfig> 成员
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<RegularExpressionValidatorConfig>");
            sb.Append("<Items>");
            foreach (KeyValuePair<string, string> Item in this.Validators)
            {
                sb.Append("<Item>");
                sb.Append("<Key>");
                sb.Append(Item.Key);
                sb.Append("</Key>");
                sb.Append("<Value>");
                sb.Append(Item.Value);
                sb.Append("</Value>");
                sb.Append("</Item>");
            }
            sb.Append("</Items>");
            sb.Append("</RegularExpressionValidatorConfig>");

            return sb.ToString();
        }

        /// <summary>
        /// 从配置文件构件对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public RegularExpressionValidatorConfig FromConfig(string Config)
        {
            RegularExpressionValidatorConfig rInfo = new RegularExpressionValidatorConfig();

            if (Config.IsEmpty())
                return rInfo;

            IList<string> _Tags = Config.GetTag("Items").GetTags("Item");

            foreach (string Item in _Tags)
                rInfo.Validators.Add(Item.GetTag("Key"), Item.GetTag("Value"));

            return rInfo;
        }

        #endregion
    }
}