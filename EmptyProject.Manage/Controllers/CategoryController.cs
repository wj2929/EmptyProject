using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using EmptyProject.DomainService.Interface;
using EmptyProject.Manage.Models;
using EmptyProject.Domain;
using BC.DDD.ObjectMapper;
using EmptyProject.Manage.Code;

namespace EmptyProject.Manage.Controllers
{
    public class CategoryController : Controller
    {

        public CategoryController(ICategoryDomainService CategoryService, ICategoryTypeDomainService CategoryTypeService)
        {
            this.CategoryService = CategoryService;
            this.CategoryTypeService = CategoryTypeService;
        }

        private readonly ICategoryDomainService CategoryService;
        private readonly ICategoryTypeDomainService CategoryTypeService;

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //GetEligibleProperties(typeof(CategoryTypeEditorModel));
            //return View(this.CategoryService.All().OrderBy(c => c.CreateDate).ToList());
            return View();
        }

        #region 分类类型
        /// <summary>
        /// 获取分类类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCategoryTypeList()
        {
            return PartialView("_GetCategoryTypeList", this.CategoryTypeService.All());
        }

        /// <summary>
        /// 创建分类类型
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryTypeCreate()
        {
            return View();
        }

        /// <summary>
        /// 创建分类类型
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CategoryTypeCreate(AddCategoryTypeModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CategoryType info = ModelInfo.Map<AddCategoryTypeModel, CategoryType>();
            this.CategoryTypeService.AddCategoryType(info);
            return this.RefurbishAndClose("CategoryType");
        }

        /// <summary>
        /// 核查分类类型Key是否存在
        /// </summary>
        /// <param name="Keycode"></param>
        /// <returns></returns>
        public JsonResult CheckCategoryTypeKeyExists(string Keycode)
        {
            return Json(!CategoryTypeService.CheckKeyExist(Keycode));
        }

        public ActionResult CategoryTypeEdit(Guid Id)
        {
            CategoryType info = this.CategoryTypeService.Single(Id);
            return View(info.Map<CategoryType, EditCategoryTypeModel>());
        }

        [HttpPost]
        public ActionResult CategoryTypeEdit(EditCategoryTypeModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CategoryType info = this.CategoryTypeService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditCategoryTypeModel, CategoryType>(info);
            this.CategoryTypeService.EditCategoryType(info);
            return this.RefurbishAndClose("CategoryType");
        }

        [HttpPost]
        public JsonResult DeleteCategoryType(Guid Id)
        {
            this.CategoryTypeService.Remove(Id);
            return Json("ok");
        } 
        #endregion

        #region 分类
        public ActionResult GetCategoryPage(string CategoryTypeKey)
        {
            return PartialView("_GetCategoryPage", CategoryTypeKey);
        }

        public ActionResult GetCategoryList(string CategoryTypeKey, Guid? ParentId)
        {
            return PartialView("_GetCategoryList", !ParentId.HasValue ?
                this.CategoryService.GetRootCategorys(CategoryTypeKey).ToList() :
                this.CategoryService.GetChildCategorys(ParentId.Value).ToList());
        }

        [HttpGet]
        public JsonResult GetCategoryData(string CategoryTypeKey, Guid? ParentId)
        {
            IList<DynatreeNode> Nodes = new List<DynatreeNode>();
            //IList<Category> AllCategorys = this.CategoryService.GetAllCategorys(CategoryTypeKey);
            if (ParentId == null)
            {
                DynatreeNode RootNode = new DynatreeNode
                {
                    title = "管理分类",
                    key = "root"
                };
                IList<Category> CategorysByLevels = this.CategoryService.GetCategorysByLevels(CategoryTypeKey, string.Empty,0,1);
                IList<Category> RootCategorys = CategorysByLevels.Where(c => c.ParentCategory_Id == null).OrderBy(c => c.OrderBy).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
                foreach (var Item in RootCategorys)
                {
                    bool hasChild = CategorysByLevels.Count(c => c.ParentCategory_Id != null && c.ParentCategory_Id.Value == Item.Id) > 0;
                    RootNode.children.Add(new DynatreeNode
                    {
                        title = Item.Name,
                        key = Item.Id.ToString(),
                        isFolder = false,
                        //expand = hasChild,
                        isLazy = hasChild
                    });
                }
                if (RootNode.children.Count > 0)
                {
                    RootNode.expand = true;
                    RootNode.isFolder = true;
                }
                Nodes.Add(RootNode);
            }
            else
            {
                Category ParentCategory = this.CategoryService.Single(ParentId.Value);
                IList<Category> CategorysByLevels = this.CategoryService.GetCategorysByLevels(CategoryTypeKey, ParentCategory.Index,ParentCategory.Level + 1, ParentCategory.Level + 2);
                IList<Category> Categorys = CategorysByLevels.Where(c => c.ParentCategory_Id != null && c.ParentCategory_Id == ParentId.Value).OrderBy(c => c.CreateDate).OrderBy(c => c.OrderBy).ToList();
                foreach (var Item in Categorys)
                {
                    bool hasChild = CategorysByLevels.Count(c => c.ParentCategory_Id != null && c.ParentCategory_Id.Value == Item.Id) > 0;
                    Nodes.Add(new DynatreeNode
                    {
                        title = Item.Name,
                        key = Item.Id.ToString(),
                        isFolder = false,
                        //expand = hasChild,
                        isLazy = hasChild
                    });
                }
            }
            return Json(Nodes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCategory(Guid Id)
        {
            this.CategoryService.Remove(Id);
            return Json("ok");
        }

        [HttpPost]
        public JsonResult DeleteCategorys(string Ids) 
        {
            if (!Ids.IsEmpty())
            {
                this.CategoryService.Removes(Ids.SplitToGuidList().ToArray());
            }
            return Json("ok");
        }
        #endregion


        #region 注释
        //List<ModelProperty> GetEligibleProperties(Type type)
        //{
        //    List<ModelProperty> results = new List<ModelProperty>();
        //    PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(type);
        //    foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //    {
        //        Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        //        int Order = 0;
        //        try
        //        {
        //            PropertyDescriptor pd = pdc[prop.Name];
        //            Attribute attribute = pd.Attributes[typeof(DisplayAttribute)];
        //            if (attribute != null)
        //                Order = ((DisplayAttribute)attribute).Order;
        //        }
        //        catch { }
        //        bool Hide = false;
        //        try
        //        {
        //            PropertyDescriptor pd = pdc[prop.Name];
        //            Attribute attribute = pd.Attributes[typeof(HiddenInputAttribute)];
        //            if (attribute != null)
        //                Hide = !((HiddenInputAttribute)attribute).DisplayValue;
        //        }
        //        catch { }

        //        //if (prop.GetGetMethod() != null && prop.GetIndexParameters().Length == 0 && IsBindableType(underlyingType))
        //        //{
        //        //    results.Add(new ModelProperty
        //        //    {
        //        //        Name = prop.Name,
        //        //        ValueExpression = "Model." + prop.Name,
        //        //        UnderlyingType = underlyingType,
        //        //        IsPrimaryKey = IsPrimaryKey(prop),
        //        //        IsReadOnly = prop.GetSetMethod() == null,
        //        //        Order = Order,
        //        //        Hide = Hide
        //        //    });
        //        //}
        //    }

        //    return results.OrderBy(r => r.OrderBy).ToList();
        //}

        //class ModelProperty
        //{
        //    public string Name { get; set; }
        //    public string ValueExpression { get; set; }
        //    public Type UnderlyingType { get; set; }
        //    public bool IsPrimaryKey { get; set; }
        //    public bool IsReadOnly { get; set; }
        //    public int Order { get; set; }
        //    public bool Hide { get; set; }
        //}
        
        #endregion
    }
}
