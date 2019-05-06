using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BC.Core;
using EmptyProject.Core;

namespace EmptyProject.Domain.Values.Category
{
    public class ProvinceCityCategoryExtended : IConfigBase<ProvinceCityCategoryExtended>, IOutHtml
    {
        #region  IConfigBase<ProvinceCityCategoryExtended>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ProvinceCityCategoryExtended>");
            sb.Append("</ProvinceCityCategoryExtended>");
            return sb.ToString();
        }

        public ProvinceCityCategoryExtended FromConfig(string Config)
        {
            if (Config.IsEmpty())
                return null;

            return new ProvinceCityCategoryExtended();
        } 
        #endregion

        #region IOutHtml
        public string ToHtml()
        {
            return string.Empty;
        } 
        #endregion
    }
}
