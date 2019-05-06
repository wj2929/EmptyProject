using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmptyProject.Manage.Models
{
    public abstract class BaseOrganModel
    {
        [Required(ErrorMessage = "必须填写名称")]
        [Display(Name = "名称", Order = 0)]
        [StringLength(100, ErrorMessage = "名称长度不能超过100个字节")]
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "联系人", Order = 0)]
        public string LinkMan { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话", Order = 0)]
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址", Order = 0)]
        public string Address { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email", Order = 0)]
        public string Email { get; set; }

        /// <summary>
        /// 首页图片Url
        /// </summary>
        [Display(Name = "首页图片Url", Order = 0)]
        public string HeadPicUrl { get; set; }

        /// <summary>
        /// 二维码Url
        /// </summary>
        [Display(Name = "二维码Url", Order = 0)]
        public string QRCodeUrl { get; set; }

        /// <summary>
        /// 机构描述
        /// </summary>
        [Display(Name = "机构描述", Order = 0)]
        public string Description { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        [Display(Name = "Logo", Order = 0)]
        [DataType("UploadLogo")]
        public string Logo { get; set; }

        /// <summary>
        /// 机构网址
        /// </summary>
        [Display(Name = "机构网址", Order = 0)]
        public string WebPageUrl { get; set; }

        /// <summary>
        /// 其他信息
        /// </summary>
        [Display(Name = "其他信息", Order = 0)]
        public string Other { get; set; }

        [Display(Name = "类目", Order = 2)]
        [DataType("CustomFormSelectListByKeycode")]
        public string CustomFormKeycode { get; set; }

        private IDictionary<string, ExtendedAttribute> _DicExtendedAttributes;
        /// <summary>
        /// ExtendedAttributes
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public IDictionary<string, ExtendedAttribute> DicExtendedAttributes
        {
            get
            {
                if (_DicExtendedAttributes == null)
                    _DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();

                return _DicExtendedAttributes;
            }
            set
            {
                _DicExtendedAttributes = value;
            }
        }
    }

    /// <summary>
    /// 新建模型
    /// </summary>
    public class AddOrganModel : BaseOrganModel
    {
        public AddOrganModel()
        {

        }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditOrganModel : BaseOrganModel
    {
        /// <summary>
        /// 要编辑信息的Id
        /// </summary>
        [GuidNotEmpty(ErrorMessage = "必须提供要编辑信息的Id")]
        [HiddenInput(DisplayValue = false)]
        public Guid EditId { get; set; }
    }
}