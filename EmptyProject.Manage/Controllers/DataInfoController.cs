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
    public class DataInfoController : Controller
    {

        public DataInfoController(ICustomFormDomainService CustomFormService,
            ICustomFormItemDomainService CustomFormItemService,
            IDataInfoDomainService DataInfoService)
        {
            this.DataInfoService = DataInfoService;
            this.CustomFormService = CustomFormService;
            this.CustomFormItemService = CustomFormItemService;
        }

        private readonly IDataInfoDomainService DataInfoService;
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

        #region DataInfo

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataInfoList(Guid CustomForm_Id, int Page=1)
        {
            CustomForm CustomForm= this.CustomFormService.Single(CustomForm_Id);
            if (CustomForm != null)
            {
                return PartialView(string.Format("_GetDataInfoList_{0}",CustomForm.Key),
                    this.DataInfoService.GetDataInfoPaging(
                        new Domain.QueryObject.DataInfoCriteria() { CustomForm_Id = CustomForm_Id }, Page));
            }
            else
                return Content("请选择类目");
        }

        public ActionResult GetExtendedAttributes(Guid? Id, string CustomFormKeycode)
        {
            IDictionary<string, ExtendedAttribute> DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
            if (Id.HasValue && !Id.Value.IsEmpty())
            {
                DataInfo DataInfo = this.DataInfoService.Single(Id.Value);
                DicExtendedAttributes = DataInfo.DicExtendedAttributes;
            }
            CustomForm CustomForm = this.CustomFormService.SingleByKeycode(CustomFormKeycode);
            IList<CustomFormItem> CustomFormItemInfos = this.CustomFormItemService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
            IList<ExtendedAttributeModel> ExtendedAttributeModels = new List<ExtendedAttributeModel>();
            foreach (var Item in CustomFormItemInfos)
            {
                ExtendedAttributeModels.Add(new ExtendedAttributeModel
                {
                    ExtendedAttribute = new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = DicExtendedAttributes.ContainsKey(Item.Key) ? DicExtendedAttributes[Item.Key].Value : ""
                    },
                    CustomFormItem = Item
                });
            }
            return PartialView("_GetExtendedAttributes", ExtendedAttributeModels);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult DataInfoCreate()
        {
            return View(new AddDataInfoModel());
        }

        /// <summary>
        /// 保存创建结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DataInfoCreate(AddDataInfoModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            DataInfo DataInfo = ModelInfo.Map<AddDataInfoModel, DataInfo>();

            //if (!DataInfo.CustomFormKeycode.IsEmpty())
            //{
            //    CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(DataInfo.CustomFormKeycode);
            //    DataInfo.CustomFormKeycode = CustomForm.Key;
            //    IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
            //    foreach (var Item in TestPaperFormItemInfos)
            //    {
            //        DataInfo.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
            //        {
            //            Key = Item.Key,
            //            Name = Item.Name,
            //            Value = Request["DicExtendedAttributes_" + Item.Key]
            //        });
            //    }
            //}

            this.DataInfoService.AddDataInfo(DataInfo);
            return this.RefurbishAndClose("DataInfo");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DataInfoEdit(Guid Id)
        {
            DataInfo DataInfo = this.DataInfoService.Single(Id);
            EditDataInfoModel ModelInfo = DataInfo.Map<DataInfo, EditDataInfoModel>();
            return View(ModelInfo);
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DataInfoEdit(EditDataInfoModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);
            DataInfo DataInfo = this.DataInfoService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditDataInfoModel, DataInfo>(DataInfo);
            //if (!DataInfo.CustomFormKeycode.IsEmpty())
            //{
            //    CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(DataInfo.CustomFormKeycode);
            //    DataInfo.CustomFormKeycode = CustomForm.Key;
            //    IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
            //    foreach (var Item in TestPaperFormItemInfos)
            //    {
            //        DataInfo.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
            //        {
            //            Key = Item.Key,
            //            Name = Item.Name,
            //            Value = Request["DicExtendedAttributes_" + Item.Key]
            //        });
            //    }
            //}
            this.DataInfoService.EditDataInfo(DataInfo);
            return this.RefurbishAndClose("DataInfo");
        }

        /// <summary>
        /// 删除自定义表单项
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDataInfo(Guid Id)
        {
            this.DataInfoService.Remove(Id);
            return Json("ok");
        }  
        #endregion



    }
}
