using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EmptyProject.Domain;
using BC.Core;
using EmptyProject.Domain.Values.Category;

namespace EmptyProject.Manage.Models
{
    public interface ICategoryModel
    {
        /// <summary>
        /// 转换为分类
        /// </summary>
        /// <returns></returns>
        Category ToCategory();
    }
    public abstract class BaseCategoryModel<TValue> : ICategoryModel
        where TValue : class,IConfigBase<TValue>, new()
    {
        public BaseCategoryModel(string Type)
        {
            this.Type = Type;
            this.Extended = new TValue();
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "名称", Order = 0)]
        public System.String Name
        {
            get 
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// ParentId
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public Guid? ParentCategory_Id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Display(Name = "排序", Order = 1)]
        public Int32 OrderBy { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Display(Name = "类型")]
        [HiddenInput(DisplayValue = false)]
        public string Type { get; set; }

        private string mExtendedConfig;
        /// <summary>
        /// 资源扩展配置信息
        /// </summary>
        public virtual string ExtendedConfig
        {
            get
            {
                if (this.mExtendedConfig.IsEmpty() && this.Extended != null)
                    this.mExtendedConfig = this.Extended.ToConfig();
                return this.mExtendedConfig;
            }
            protected set
            {
                this.mExtendedConfig = value;
                this.Extended = new TValue().FromConfig(this.mExtendedConfig);
                if (this.Extended == null)
                    this.Extended = new TValue();
            }
        }
        /// <summary>
        /// 扩展属性对象
        /// </summary>
        public TValue Extended { get; set; }

        [Display(Name = "标识", Order = 5)]
        public string Flag { get; set; }

        /// <summary>
        /// 转换为分类对象
        /// </summary>
        /// <returns></returns>
        public Category ToCategory()
        {
            string ExtendedConfig = this.Extended == null ? string.Empty : this.Extended.ToConfig();
            return new Category(this.Name, this.OrderBy, this.ParentCategory_Id, this.Type, this.Flag, ExtendedConfig);
        }
    }

    public class EditCategoryModel<TValue> : BaseCategoryModel<TValue>
           where TValue : class,IConfigBase<TValue>, new()
    {
        public EditCategoryModel(string Type)
            : base(Type)
        {
        }

        /// <summary>
        /// 要编辑的信息的Id
        /// </summary>
        [GuidNotEmpty(ErrorMessage = "必须提供要编辑信息的Id")]
        [HiddenInput(DisplayValue = false)]
        public Guid EditId { get; set; }

        //public void SetExtendedConfig(string ExtendedConfig)
        //{
        //    this.ExtendedConfig = ExtendedConfig;
        //}
    }

    #region ProvinceCityCategory

    public class AddProvinceCityCategoryModel : BaseCategoryModel<ProvinceCityCategoryExtended>
    {
        public AddProvinceCityCategoryModel()
            : base("ProvinceCity")
        {

        }
    }

    public class EditProvinceCityCateogryModel : EditCategoryModel<ProvinceCityCategoryExtended>
    {
        public EditProvinceCityCateogryModel()
            : base("ProvinceCity")
        {

        }
    }
    #endregion

    #region AreaGroupCategory

    public class AddAreaGroupCategoryModel : BaseCategoryModel<AreaGroupCategoryExtended>
    {
        //private CheckBoxGroupModel _MainProvinceCityCategoryCheckBoxGroupModel;

        // [Display(Name = "省市")]
        //public CheckBoxGroupModel MainProvinceCityCategoryCheckBoxGroupModel
        //{
        //    get 
        //    {
        //        return _MainProvinceCityCategoryCheckBoxGroupModel;
        //    }
        //    set 
        //    {
        //        _MainProvinceCityCategoryCheckBoxGroupModel = value;
        //    }
        //}

        public AddAreaGroupCategoryModel()
            : base("AreaGroup")
        {
            Extended = new AreaGroupCategoryExtended();
            Extended.ProvinceCityCategoryIds = new List<Guid>();
        }
    }

    public class EditAreaGroupCateogryModel : EditCategoryModel<AreaGroupCategoryExtended>
    {

        public EditAreaGroupCateogryModel()
            : base("AreaGroup")
        {
            Extended = new AreaGroupCategoryExtended();
        }
    }
    #endregion

}