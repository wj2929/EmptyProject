using ZSCX.Domain;
using ZSCX.DomainService.Interface;
using ZSCX.Manage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BC.DDD.ObjectMapper;
using ZSCX.Manage.Code;

namespace ZSCX.Manage.Controllers
{
    public class TestPaperMController : Controller
    {
        public TestPaperMController(ITestPaperDomainService TestPaperDomainService,
            ICategoryDomainService CategoryDomainService,
            ITestPaper_QuestionDomainService TestPaper_QuestionDomainService,
            ICustomFormDomainService CustomFormDomainService,
            ICustomFormItemDomainService CustomFormItemDomainService)
        {
            this.TestPaperDomainService = TestPaperDomainService;
            this.CategoryDomainService = CategoryDomainService;
            this.TestPaper_QuestionDomainService = TestPaper_QuestionDomainService;
            this.CustomFormDomainService = CustomFormDomainService;
            this.CustomFormItemDomainService = CustomFormItemDomainService;
        }
        private readonly ITestPaperDomainService TestPaperDomainService;
        private readonly ICategoryDomainService CategoryDomainService;
        private readonly ITestPaper_QuestionDomainService TestPaper_QuestionDomainService;
        private readonly ICustomFormDomainService CustomFormDomainService;
        private readonly ICustomFormItemDomainService CustomFormItemDomainService;

        /// <summary>
        /// 获取商品主分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMainCategoryList()
        {
            return PartialView("_GetCategoryList", this.CategoryDomainService.GetRootCategorys("QuestionType").OrderBy(c => c.OrderBy).ToList());
        }

        /// <summary>
        /// 获取商品次分类列表
        /// </summary>
        /// <param name="ParentCategoryId"></param>
        /// <returns></returns>
        public ActionResult GetChildCategoryList(Guid? ParentCategoryId)
        {
            if (!ParentCategoryId.HasValue || ParentCategoryId.Value.IsEmpty())
                return PartialView("_GetCategoryList", this.CategoryDomainService.GetAllCategorys("QuestionType").Where(c => c.Level > 0).OrderBy(c => c.OrderBy).ToList());
            else
                return PartialView("_GetCategoryList", this.CategoryDomainService.GetChildCategorys(ParentCategoryId.Value));
        }

        /// <summary>
        /// 获取Goods列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList(Guid? CategoryId, string Name, int Page)
        {
            return PartialView("_GetList",
                this.TestPaperDomainService.GetTestPaperPaging(
                    new Domain.QueryObject.TestPaperCriteria() { Category_Id = CategoryId.HasValue ? CategoryId.Value : Guid.Empty, Title = Name }, Page));
        }


        // GET: TestPaper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetExtendedAttributes(Guid? Id, string CustomFormKeycode)
        {
            IDictionary<string, ExtendedAttribute> DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
            if (Id.HasValue && !Id.Value.IsEmpty())
            {
                TestPaper TestQuestionInfo = this.TestPaperDomainService.Single(Id.Value);
                DicExtendedAttributes = TestQuestionInfo.DicExtendedAttributes;
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
        public ActionResult TestPaperCreate()
        {
            return View(new AddTestPaperModel());
        }

        /// <summary>
        /// 保存创建结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestPaperCreate(AddTestPaperModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            TestPaper TestPaper = ModelInfo.Map<AddTestPaperModel, TestPaper>();
            if (!TestPaper.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(TestPaper.CustomFormKeycode);
                TestPaper.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestPaperFormItemInfos)
                {
                    TestPaper.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }

            this.TestPaperDomainService.AddTestPaper(TestPaper);
            return this.RefurbishAndClose("TestPaper");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult TestPaperEdit(Guid Id)
        {
            TestPaper TestPaper = this.TestPaperDomainService.Single(Id);
            EditTestPaperModel ModelInfo = TestPaper.Map<TestPaper, EditTestPaperModel>();
            return View(ModelInfo);
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestPaperEdit(EditTestPaperModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);
            TestPaper TestPaper = this.TestPaperDomainService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditTestPaperModel, TestPaper>(TestPaper);
            if (!TestPaper.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(TestPaper.CustomFormKeycode);
                TestPaper.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestPaperFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestPaperFormItemInfos)
                {
                    TestPaper.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }
            this.TestPaperDomainService.EditTestPaper(TestPaper);
            return this.RefurbishAndClose("TestPaper");
        }

        /// <summary>
        /// 获取Goods列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJoinList(Guid? CategoryId, string Name, int Page)
        {
            return PartialView("_GetJoinList",
                this.TestPaperDomainService.GetTestPaperPaging(
                    new Domain.QueryObject.TestPaperCriteria() { Category_Id = CategoryId.HasValue ? CategoryId.Value : Guid.Empty, Title = Name }, Page));
        }


        // GET: TestPaper
        public ActionResult Join(Guid TestQuestionId)
        {
            ViewBag.TestQuestionId = TestQuestionId;
            return View();
        }

        [HttpPost]
        public JsonResult TestQuestionJoin(Guid Id, Guid TestQuestionId)
        {
            if (this.TestPaper_QuestionDomainService.Count(new Domain.QueryObject.TestPaper_QuestionCriteria() { TestPaperId = Id, TestQuestionId = TestQuestionId }) == 0)
                this.TestPaper_QuestionDomainService.AddTestPaper_Question(new Domain.TestPaper_Question()
                {
                    TestPaper_Id = Id,
                    TestQuestion_Id = TestQuestionId
                });

            return Json(BC.Core.BaseReturnInfo.Success(string.Empty));
        }

        /// <summary>
        /// 获取"自动完成"商品列表
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAutoCompleteTestPaperNames(string q, int limit)
        {
            return Json(string.Join("\n", this.TestPaperDomainService.GetList(new Domain.QueryObject.TestPaperCriteria() { Title = q }, 1, limit).Select(g => g.Title).ToArray()), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListByStage(Guid StageId)
        {
            return PartialView("_GetListByStage", this.TestPaperDomainService.GetList(new Domain.QueryObject.TestPaperCriteria() { StageInfo_Id = StageId }));
        }

        public ActionResult ListByStage(Guid StageId)
        {
            ViewBag.StageId = StageId;
            return View();
        }

        [HttpPost]
        public JsonResult RemoveTestPaperFromStage(Guid Id, Guid StageId)
        {
            TestPaper TestPaper = this.TestPaperDomainService.Single(Id);
            if (TestPaper != null)
            {
                TestPaper.StageInfo_Id = Guid.Empty;
                this.TestPaperDomainService.EditTestPaper(TestPaper);
            }

            return Json(BC.Core.BaseReturnInfo.Success(string.Empty));
        }

        /// <summary>
        /// 批量设置推出
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BatchIntroductions(string Ids)
        {
            if (!Ids.IsEmpty())
            {
                this.TestPaperDomainService.BatchIntroductions(Ids.SplitToGuidList().ToArray());
            }
            return Json("ok");
        }


        /// <summary>
        /// 批量设置推荐
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BatchRecommonds(string Ids)
        {
            if (!Ids.IsEmpty())
            {
                this.TestPaperDomainService.BatchRecommonds(Ids.SplitToGuidList().ToArray());
            }
            return Json("ok");
        }


    }
}