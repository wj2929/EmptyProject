using BC.Core;
using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class CustomFormItem : EntityWithGuid, IConfigBase<CustomFormItem>
    {
        public CustomFormItem()
        {
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "必须填写名称")]
        [StringLength(30, ErrorMessage = "名称长度不能超过30个字节")]
        public string Name { get; set; }
        /// <summary>
        /// 键值
        /// </summary>
        [Display(Name = "键值")]
        [Required(ErrorMessage = "必须填写键值")]
        [StringLength(50, ErrorMessage = "键值长度不能超过50个字节")]
        public string Key { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(255, ErrorMessage = "描述长度不能超过255个字节")]
        [Display(Name = "描述")]
        public string Description { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool Enabled { get; set; }
        /// <summary>
        /// 接收附件信息
        /// </summary>
        [Display(Name = "接收附件信息")]
        public bool ReceiveAttachment { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "必须设置排序值")]
        public int Order { get; set; }
        /// <summary>
        /// 验证设置
        /// </summary>
        public CustomValidation ValidationConfig { get; set; }
        /// <summary>
        /// 所属自定义表单
        /// </summary>
        public virtual CustomForm CustomForm { get; set; }

        public Guid CustomForm_Id { get; set; }

        /// <summary>
        /// FormType
        /// </summary>
        public int FormType { get; set; }
        public FormItemType FormItemType
        {
            get
            {
                return (FormItemType)FormType;
            }
        }

        /// <summary>
        /// MoreSelect
        /// </summary>
        public bool MoreSelect { get; set; }

        /// <summary>
        /// OptionText
        /// </summary>
        public string OptionText { get; set; }


        #region IConfigBase<CustomFormItem>成员
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<CustomFormItem>");
            sb.Append("<Name>");
            sb.Append(this.Name);
            sb.Append("</Name>");
            sb.Append("<Key>");
            sb.Append(this.Key);
            sb.Append("</Key>");
            sb.Append("<Description>");
            sb.Append(this.Description);
            sb.Append("</Description>");
            sb.Append("<Enabled>");
            sb.Append(this.Enabled.ToString());
            sb.Append("</Enabled>");
            sb.Append("<ReceiveAttachment>");
            sb.Append(this.ReceiveAttachment.ToString());
            sb.Append("</ReceiveAttachment>");
            sb.Append("<Order>");
            sb.Append(this.Order.ToString());
            sb.Append("</Order>");
            sb.Append("<CustomValidation>");
            sb.Append(this.ValidationConfig.ToConfig());
            sb.Append("</CustomValidation>");
            sb.Append("</CustomFormItem>");
            return sb.ToString();
        }

        public CustomFormItem FromConfig(string Config)
        {
            return new CustomFormItem()
            {
                Name = Config.GetTag("Name"),
                Key = Config.GetTag("Key"),
                Description = Config.GetTag("Description"),
                Enabled = Config.GetTag("Enabled").BoolByString(),
                ReceiveAttachment = Config.GetTag("ReceiveAttachment").BoolByString(),
                Order = Config.GetTag("Order").IntByString(),
                ValidationConfig = new CustomValidation().FromConfig(Config.GetTag("CustomValidation"))
            };
        }
        #endregion
    }

    public enum FormItemType
    {
        单行文本框 = 0,
        多行文本框 = 1,
        列表框 = 2,
    }
}
