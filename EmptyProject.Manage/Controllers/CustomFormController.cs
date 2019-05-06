using EmptyProject.Domain;
using EmptyProject.DomainService.Interface;
using EmptyProject.Manage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BC.DDD.ObjectMapper;
using EmptyProject.Manage.Code;

namespace EmptyProject.Manage.Controllers
{
    public class CustomFormController : Controller
    {

        public CustomFormController(ICustomFormDomainService CustomFormService,
            ICustomFormItemDomainService CustomFormItemService)
        {
            this.CustomFormService = CustomFormService;
            this.CustomFormItemService = CustomFormItemService;
        }

        private readonly ICustomFormDomainService CustomFormService;
        private readonly ICustomFormItemDomainService CustomFormItemService;

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 类目
        /// <summary>
        /// 获取商品类目列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomFormList()
        {
            return PartialView("_GetCustomFormList", this.CustomFormService.All());
        }

        /// <summary>
        /// 创建类目
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public ActionResult CustomFormCreate()
        {
            return View(new AddCustomFormModel());
        }

        /// <summary>
        /// 创建类目
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomFormCreate(AddCustomFormModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CustomForm info = ModelInfo.Map<AddCustomFormModel, CustomForm>();
            this.CustomFormService.AddCustomForm(info);
            return this.RefurbishAndClose("CustomForm");

        }

        /// <summary>
        /// 编辑类目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CustomFormEdit(Guid Id)
        {
            CustomForm info = this.CustomFormService.Single(Id);
            return View(info.Map<CustomForm, EditCustomFormModel>());
        }

        /// <summary>
        /// 编辑类目
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomFormEdit(EditCustomFormModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CustomForm info = this.CustomFormService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditCustomFormModel, CustomForm>(info);
            this.CustomFormService.EditCustomForm(info);
            return this.RefurbishAndClose("CustomForm");
        }

        /// <summary>
        /// 删除类目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCustomForm(Guid Id)
        {
            this.CustomFormService.Remove(Id);
            return Json("ok");
        } 
        #endregion

        #region 自定义表单项

        public ActionResult GetCustomFormItemList(Guid CustomFormId)
        {
            return PartialView("_GetCustomFormItemList", this.CustomFormItemService.GetCustomFormItems(CustomFormId));
        }

        /// <summary>
        /// 创建自定义表单项
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public ActionResult CustomFormItemCreate(Guid CustomFormId)
        {
            return View(new AddCustomFormItemModel { CustomFormId = CustomFormId});
        }

        /// <summary>
        /// 创建自定义表单项
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomFormItemCreate(AddCustomFormItemModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CustomFormItem info = ModelInfo.Map<AddCustomFormItemModel, CustomFormItem>();
            this.CustomFormItemService.AddCustomFormItem(info);
            return this.RefurbishAndClose("CustomFormItem");

        }

        /// <summary>
        /// 编辑自定义表单项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CustomFormItemEdit(Guid Id)
        {
            CustomFormItem info = this.CustomFormItemService.Single(Id);
            EditCustomFormItemModel EditCustomFormItemModel = info.Map<CustomFormItem, EditCustomFormItemModel>();
            EditCustomFormItemModel.EditId = Id;
            return View(EditCustomFormItemModel);
        }

        /// <summary>
        /// 编辑自定义表单项
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomFormItemEdit(EditCustomFormItemModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            CustomFormItem info = this.CustomFormItemService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditCustomFormItemModel, CustomFormItem>(info);
            this.CustomFormItemService.EditCustomFormItem(info);
            return this.RefurbishAndClose("CustomFormItem");
        }

        /// <summary>
        /// 删除自定义表单项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCustomFormItem(Guid Id)
        {
            this.CustomFormItemService.Remove(Id);
            return Json("ok");
        }  
        #endregion


        [HttpPost]
        public JsonResult SaveOrder(Guid CustomFormId, Guid SortId, Guid PreviousId)
        {
            this.CustomFormItemService.SaveOrder(CustomFormId, SortId, PreviousId);
            return Json(BC.Core.BaseReturnInfo.Success(string.Empty));
        }

    }
}
