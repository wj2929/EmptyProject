using BC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    /// <summary>
    /// 扩展验证
    /// </summary>
    public class CustomValidation : IConfigBase<CustomValidation>
    {
        /// <summary>
        /// 必填项
        /// </summary>
        [Display(Name = "是否为必填项")]
        public bool IsMust { get; set; }
        /// <summary>
        /// 扩展验证
        /// </summary>
        [Display(Name = "是否启用扩展验证")]
        public bool AllowExtensionValidation { get; set; }
        /// <summary>
        /// 格式验证
        /// </summary>
        [Display(Name = "扩展验证设置")]
        [StringLength(1000, ErrorMessage = "扩展验证设置不能超过1000个字节")]
        public string RegularExpressionValidator { get; set; }
        /// <summary>
        /// 出错时显示的信息
        /// </summary>
        [Display(Name = "出错时显示的信息")]
        [StringLength(300, ErrorMessage = "出错信息不能超过300个字节")]
        public string ErrorMessage { get; set; }

        #region IConfigBase<CustomFormItem> 成员
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<IsMust>");
            sb.Append(this.IsMust);
            sb.Append("</IsMust>");
            sb.Append("<AllowExtensionValidation>");
            sb.Append(this.AllowExtensionValidation.ToString());
            sb.Append("</AllowExtensionValidation>");
            sb.Append("<RegularExpressionValidator>");
            sb.Append(this.RegularExpressionValidator);
            sb.Append("</RegularExpressionValidator>");
            sb.Append("<ErrorMessage>");
            sb.Append(this.ErrorMessage);
            sb.Append("</ErrorMessage>");
            return sb.ToString();
        }

        public CustomValidation FromConfig(string Config)
        {
            return new CustomValidation()
            {
                IsMust = Config.GetTag("IsMust").BoolByString(),
                AllowExtensionValidation = Config.GetTag("AllowExtensionValidation").BoolByString(),
                RegularExpressionValidator = Config.GetTag("RegularExpressionValidator"),
                ErrorMessage = Config.GetTag("ErrorMessage")
            };
        }
        #endregion
    }
}
