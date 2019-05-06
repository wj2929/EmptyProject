using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EmptyProject.Manage.Models
{
    public abstract class BaseDataInfoModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必须填写名称")]
        [Display(Name = "名称", Order = 0)]
        [StringLength(100, ErrorMessage = "名称长度不能超过100个字节")]
        public string Name { get; set; }


        /// <summary>
        /// 商品类目Id
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public Guid CustomFormId { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>
        //[Required(ErrorMessage = "必须填写排序")]
        //[Display(Name = "排序",Order = 2)]
        //public int Order { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述", Order = 3)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "启用", Order = 0)]
        public bool Enabled { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        [Display(Name = "表单类型", Order = 4)]
        public int FormType { get; set; }

        /// <summary>
        /// 预设内容
        /// </summary>
        [Display(Name = "预设内容", Order = 5)]
        public string OptionText { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        [Display(Name = "是否多选", Order = 6)]
        public bool MoreSelect { get; set; }

        /// <summary>
        /// 必填
        /// </summary>
        [Display(Name = "必填", Order = 7)]
        public bool ValidationConfig_Required { get; set; }

        /// <summary>
        /// 启用扩展验证
        /// </summary>
        [Display(Name = "启用扩展验证", Order = 8)]
        public bool ValidationConfig_AllowExtensionValidation { get; set; }

        /// <summary>
        /// 扩展验证设置
        /// </summary>
        [Display(Name = "扩展验证设置", Order = 9)]
        public string ValidationConfig_RegularExpressionValidator { get; set; }

        /// <summary>
        /// 出错时显示的信息
        /// </summary>
        [Display(Name = "出错时显示的信息", Order = 10)]
        public string ValidationConfig_ErrorMessage { get; set; }
    }

    /// <summary>
    /// 新建模型
    /// </summary>
    public class AddDataInfoModel : BaseDataInfoModel
    {
        /// <summary>
        /// 键
        /// </summary>
        [Required(ErrorMessage = "必须填写关键字")]
        [Display(Name = "关键字", Description = "区别分类类型关键字", Order = 1)]
        [StringLength(50, ErrorMessage = "关键字长度不能超过50个字节")]
        public string Key { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditDataInfoModel : BaseDataInfoModel
    {
        /// <summary>
        /// 要编辑信息的Id
        /// </summary>
        [GuidNotEmpty(ErrorMessage = "必须提供要编辑信息的Id")]
        [HiddenInput(DisplayValue = false)]
        public Guid EditId { get; set; }
    }
}