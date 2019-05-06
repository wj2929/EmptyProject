using AutoMapper;
using BC.Core;
using BC.DDD.IoC;
using EmptyProject.Domain;
using EmptyProject.Domain.Values.Configs;
using EmptyProject.Manage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using EmptyProject.Manage.Code;
using EmptyProject.DomainService.Interface;
namespace EmptyProject.Manage.Code
{
    public class GlobalController
    {
        private static IDependencyResolver _IoC;
        /// <summary>
        /// 依赖注入
        /// </summary>
        public static IDependencyResolver IoC
        {
            get
            {
                if (_IoC == null)
                    _IoC = IoCManage.Current;

                return _IoC;
            }
        }

        static private IConfigAccessHelper<RegularExpressionValidatorConfig> _ExpressionValidatorConfig;
        /// <summary>
        /// 扩展验证规则
        /// </summary>
        static public IConfigAccessHelper<RegularExpressionValidatorConfig> ExpressionValidatorConfig
        {
            get
            {
                if (_ExpressionValidatorConfig == null)
                    _ExpressionValidatorConfig = new LocalConfigurationAccessHelper<RegularExpressionValidatorConfig>("RegularExpressionValidatorConfig");

                return _ExpressionValidatorConfig;
            }
        }

        static private IConfigAccessHelper<UploadConfig> _UploadConfig;
        /// <summary>
        /// 
        /// </summary>
        static public IConfigAccessHelper<UploadConfig> UploadConfig
        {
            get
            {
                if (_UploadConfig == null)
                    _UploadConfig = new LocalConfigurationAccessHelper<UploadConfig>("UploadConfig");

                return _UploadConfig;
            }
        }


        #region 下拉分类项
        /// <summary>
        /// 获取平面分类列表选择项
        /// </summary>
        /// <param name="CategoryTypeKey"></param>
        /// <returns></returns>
        public static IList<System.Web.Mvc.SelectListItem> GetFlagCategorySelectListItems(string CategoryTypeKey)
        {
            IList<System.Web.Mvc.SelectListItem> FlagCategoryItems = new List<System.Web.Mvc.SelectListItem>();
            ICategoryDomainService CategoryService = IoC.Resolve<ICategoryDomainService>();
            IList<Category> AllCategorys = CategoryService.GetAllCategorys(CategoryTypeKey);
            IteraCategorys(AllCategorys, null, FlagCategoryItems);
            FlagCategoryItems.Insert(0, new System.Web.Mvc.SelectListItem
            {
                Text = "请选择",
                Value = ""
            });
            return FlagCategoryItems;
        }

        static void IteraCategorys(IList<Category> AllCategorys, Guid? ParentId, IList<System.Web.Mvc.SelectListItem> FlagCategoryItems)
        {
            IList<Category> Infos = AllCategorys.Where(c => c.ParentCategory_Id == ParentId).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
            foreach (var Item in Infos)
            {
                FlagCategoryItems.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = "".PadLeft(Item.Level * 2, '-') + Item.Name,
                    Value = Item.Id.ToString(),
                    Selected = false
                });
                IteraCategorys(AllCategorys, Item.Id, FlagCategoryItems);
            }
        }
        #endregion


        public static void InjectCustomMap()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;


                cfg.CreateMap<AddCustomFormItemModel, CustomFormItem>()
                    .ForMember(t => t.ValidationConfig, dto => dto.MapFrom(a => new CustomValidation()
                    {
                        IsMust = a.ValidationConfig_Required,
                        ErrorMessage = a.ValidationConfig_ErrorMessage,
                        AllowExtensionValidation = a.ValidationConfig_AllowExtensionValidation,
                        RegularExpressionValidator = a.ValidationConfig_RegularExpressionValidator
                    }));

                cfg.CreateMap<CustomForm, EditCustomFormModel>()
                    .ForMember(t => t.EditId, opt => opt.MapFrom(b => b.Id));

                cfg.CreateMap<CustomFormItem, EditCustomFormItemModel>()
                    .ForMember(t => t.EditId, opt => opt.MapFrom(b => b.Id));

                cfg.CreateMap<EditCustomFormItemModel, CustomFormItem>()
                    .ForMember(t => t.ValidationConfig, dto => dto.MapFrom(a => new CustomValidation()
                    {
                        IsMust = a.ValidationConfig_Required,
                        ErrorMessage = a.ValidationConfig_ErrorMessage,
                        AllowExtensionValidation = a.ValidationConfig_AllowExtensionValidation,
                        RegularExpressionValidator = a.ValidationConfig_RegularExpressionValidator
                    }));

                

                cfg.IgnoreUnmapped();

                cfg.ValidateInlineMaps = false;


                //cfg.CreateMap<AddCustomFormItemModel, CustomFormItem>()
                //    .ForMember(t => t.ValidationConfig, dto => dto.MapFrom(t => null));

                //cfg.CreateMap<HanYu, HanYuItemConfig>()
                //    .ForMember(t => t.JiBenShiYi, dto => dto.MapFrom(t => t.JiBenShiYi.Replace("\n", string.Empty)))
                //    .ForMember(t => t.XiangXiShiYi, dto => dto.MapFrom(t => t.XiangXiShiYi.Replace("\n", string.Empty)));
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}