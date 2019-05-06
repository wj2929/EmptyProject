using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using BC.Core;
using EmptyProject.Core;

namespace EmptyProject.Domain.Values.Category
{
    public class AreaGroupCategoryExtended : IConfigBase<AreaGroupCategoryExtended>,IOutHtml
    {
        [Required]
        [Display(Name = "省市")]
        [DataType("MainProvinceCityCategorySelectList")]
        public IList<Guid> ProvinceCityCategoryIds { get; set; }

        public IList<string> ProvinceCityCategoryNames { get; set; }

        public AreaGroupCategoryExtended()
        {
            ProvinceCityCategoryIds = new List<Guid>();
            ProvinceCityCategoryNames = new List<string>();
        }

        #region IConfigBase<AreaGroupCategoryExtended>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<AreaGroupCategoryExtended>");
            sb.Append("<ProvinceCityCategoryIds>");
            ProvinceCityCategoryIds = ProvinceCityCategoryIds ?? new List<Guid>();
            foreach (var Item in ProvinceCityCategoryIds)
            {
                sb.Append("<ProvinceCityCategoryId>");
                sb.Append(Item.ToString());
                sb.Append("</ProvinceCityCategoryId>");
            }
            sb.Append("</ProvinceCityCategoryIds>");

            sb.Append("<ProvinceCityCategoryNames>");
            ProvinceCityCategoryNames = ProvinceCityCategoryNames ?? new List<string>();
            foreach (var Item in ProvinceCityCategoryNames)
            {
                sb.Append("<ProvinceCityCategoryName>");
                sb.Append(Item.ToString());
                sb.Append("</ProvinceCityCategoryName>");
            }
            sb.Append("</ProvinceCityCategoryNames>");

            sb.Append("</AreaGroupCategoryExtended>");
            return sb.ToString();
        }

        public AreaGroupCategoryExtended FromConfig(string Config)
        {
            if (Config.IsEmpty())
                return null;

            return new AreaGroupCategoryExtended
            {
                ProvinceCityCategoryIds = Config.GetTag("ProvinceCityCategoryIds").GetTags("ProvinceCityCategoryId").GuidByString(),
                ProvinceCityCategoryNames = Config.GetTag("ProvinceCityCategoryNames").GetTags("ProvinceCityCategoryName")
            };
        } 
        #endregion

        #region IOutHtml
        public string ToHtml()
        {
            return string.Format("省市：{0}", string.Join(",", ProvinceCityCategoryNames.ToArray()));
        } 
        #endregion
    }
}
