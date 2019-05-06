using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmptyProject.Manage.Models
{
    public abstract class BaseCategoryTypeModel
    {
        /// <summary>
        /// 分类类型名称
        /// </summary>
        [Required(ErrorMessage = "必须填写名称")]
        [Display(Name = "名称",Order=0)]
        [StringLength(100, ErrorMessage = "名称长度不能超过100个字节")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 分类类型新建模型
    /// </summary>
    public class AddCategoryTypeModel : BaseCategoryTypeModel
    {
        /// <summary>
        /// 键值
        /// </summary>
        [Required(ErrorMessage = "必须填写关键字")]
        [Display(Name = "关键字", Description = "区别分类类型关键字",Order=1)]
        [StringLength(50, ErrorMessage = "关键字长度不能超过50个字节")]
        [Remote("CheckCategoryTypeKeyExists", "Category", ErrorMessage = "分类类型键值已经存在", HttpMethod = "POST")]
        public string Keycode { get; set; }
    }

    /// <summary>
    /// 站点编辑模型
    /// </summary>
    public class EditCategoryTypeModel : BaseCategoryTypeModel
    {
        /// <summary>
        /// 要编辑信息的Id
        /// </summary>
        [GuidNotEmpty(ErrorMessage = "必须提供要编辑信息的Id")]
        [HiddenInput(DisplayValue = false)]
        public Guid EditId { get; set; }
    }
}