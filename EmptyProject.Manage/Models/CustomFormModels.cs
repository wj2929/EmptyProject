using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmptyProject.Manage.Models
{
    public abstract class BaseCustomFormModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必须填写名称")]
        [Display(Name = "名称", Order = 0)]
        [StringLength(100, ErrorMessage = "名称长度不能超过100个字节")]
        public string Name { get; set; }

    }

    /// <summary>
    /// 新建模型
    /// </summary>
    public class AddCustomFormModel : BaseCustomFormModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required(ErrorMessage = "必须填写关键字")]
        [Display(Name = "关键字", Order = 0)]
        [StringLength(100, ErrorMessage = "关键字长度不能超过100个字节")]
        public string Key { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditCustomFormModel : BaseCustomFormModel
    {
        /// <summary>
        /// 要编辑信息的Id
        /// </summary>
        [GuidNotEmpty(ErrorMessage = "必须提供要编辑信息的Id")]
        [HiddenInput(DisplayValue = false)]
        public Guid EditId { get; set; }
    }
}