using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DM.DomainService.Interface;
using DM.Domain;
using DM.Manage.Code;
using DM.Manage.Models;
using BC.DDD.ObjectMapper;

namespace DM.Manage.Controllers
{
    public class OrganController : Controller
    {
        public OrganController(IOrganDomainService OrganDomainService, 
            ICustomFormDomainService CustomFormDomainService,
            ICustomFormItemDomainService CustomFormItemDomainService)
        {
            this.OrganDomainService = OrganDomainService;
            this.CustomFormDomainService = CustomFormDomainService;
            this.CustomFormItemDomainService = CustomFormItemDomainService;
        }
        private readonly IOrganDomainService OrganDomainService;
        private readonly ICustomFormDomainService CustomFormDomainService;
        private readonly ICustomFormItemDomainService CustomFormItemDomainService;

        // GET: Organ
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取Goods列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList(string Name, int Page)
        {
            return PartialView("_GetList",
                this.OrganDomainService.GetOrganPaging(
                    new Domain.QueryObject.OrganCriteria() { Name = Name }, Page));
        }

        public ActionResult GetExtendedAttributes(Guid? Id, string CustomFormKeycode)
        {
            IDictionary<string, ExtendedAttribute> DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
            if (Id.HasValue && !Id.Value.IsEmpty())
            {
                Organ Organ = this.OrganDomainService.Single(Id.Value);
                DicExtendedAttributes = Organ.DicExtendedAttributes;
            }
            CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(CustomFormKeycode);
            IList<CustomFormItem> CustomFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
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
        public ActionResult OrganCreate()
        {
            return View(new AddOrganModel());
        }

        /// <summary>
        /// 保存创建结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OrganCreate(AddOrganModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            Organ Organ = ModelInfo.Map<AddOrganModel, Organ>();

            if (!Organ.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(Organ.CustomFormKeycode);
                Organ.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestPaperFormItemInfos)
                {
                    Organ.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }

            this.OrganDomainService.AddOrgan(Organ);
            return this.RefurbishAndClose("Organ");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult OrganEdit(Guid Id)
        {
            Organ Organ = this.OrganDomainService.Single(Id);
            EditOrganModel ModelInfo = Organ.Map<Organ, EditOrganModel>();
            return View(ModelInfo);
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OrganEdit(EditOrganModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);
            Organ Organ = this.OrganDomainService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditOrganModel, Organ>(Organ);
            if (!Organ.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(Organ.CustomFormKeycode);
                Organ.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestPaperFormItemInfos)
                {
                    Organ.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }
            this.OrganDomainService.EditOrgan(Organ);
            return this.RefurbishAndClose("Organ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteOrgan(Guid Id)
        {
            this.OrganDomainService.Remove(Id);
            return Json("ok");
        }

        /// <summary>
        /// 获取"自动完成"列表
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAutoCompleteOrganNames(string q, int limit)
        {
            return Json(string.Join("\n", this.OrganDomainService.GetList(new Domain.QueryObject.OrganCriteria() { Name = q }, 1, limit).Select(g => g.Name).ToArray()), JsonRequestBehavior.AllowGet);
        } 

    }
}