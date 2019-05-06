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
    public class TestQuestionController : Controller
    {
        public TestQuestionController(ITestQuestionDomainService TestQuestionDomainService,
            ITestPaper_QuestionDomainService TestPaper_QuestionDomainService,
            ITestQuestionItemDomainService TestQuestionItemDomainService, 
            ICategoryDomainService CategoryDomainService, 
            ICustomFormItemDomainService CustomFormItemDomainService,
            ICustomFormDomainService CustomFormDomainService,
            IAttachmentDomainService AttachmentDomainService)
        {
            this.TestQuestionDomainService = TestQuestionDomainService;
            this.TestPaper_QuestionDomainService = TestPaper_QuestionDomainService;
            this.TestQuestionItemDomainService = TestQuestionItemDomainService;
            this.CategoryDomainService = CategoryDomainService;
            this.CustomFormItemDomainService = CustomFormItemDomainService;
            this.CustomFormDomainService = CustomFormDomainService;
            this.AttachmentDomainService = AttachmentDomainService;
        }
        private readonly ITestQuestionDomainService TestQuestionDomainService;
        private readonly ITestPaper_QuestionDomainService TestPaper_QuestionDomainService;
        private readonly ITestQuestionItemDomainService TestQuestionItemDomainService;
        private readonly ICategoryDomainService CategoryDomainService;
        private readonly ICustomFormItemDomainService CustomFormItemDomainService;
        private readonly ICustomFormDomainService CustomFormDomainService;
        private readonly IAttachmentDomainService AttachmentDomainService;
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
        /// 获取TestQuestion列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList(Guid? CategoryId, string Name, int Page)
        {
            CategoryId = CategoryId ?? Guid.Empty;
            return PartialView("_GetList", this.TestQuestionDomainService.GetTestQuestionPaging(new Domain.QueryObject.TestQuestionCriteria() {Name=Name,CategoryId=CategoryId.Value },Page));
        }


        public ActionResult GetExtendedAttributes(Guid? Id, string CustomFormKeycode)
        {
            IDictionary<string, ExtendedAttribute> DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
            if (Id.HasValue && !Id.Value.IsEmpty())
            {
                TestQuestion TestQuestionInfo = this.TestQuestionDomainService.Single(Id.Value);
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
            //return PartialView("_GetExtendedAttributes", this.TestQuestionFormItemService.GetTestQuestionFormItems(TestQuestionFormId).Where(g=>g.Enabled).ToList());
        }

        public ActionResult GetListByTestPaper(Guid TestPaperId)
        {
            IList<TestPaper_Question> TestPaper_Questions = this.TestPaper_QuestionDomainService.GetList(new Domain.QueryObject.TestPaper_QuestionCriteria() { TestPaperId = TestPaperId }).OrderBy(t => t.OrderBy).ToList();
            IList<TestQuestion> TestQuestions = this.TestQuestionDomainService.GetList(new Domain.QueryObject.TestQuestionCriteria() { Ids = TestPaper_Questions.Select(t => t.TestQuestion_Id).ToArray() });
            IList<TestQuestion> ReturnTestQuestions = new List<TestQuestion>();
            foreach (var Item in TestPaper_Questions)
            {
                ReturnTestQuestions.Add(TestQuestions.Single(t => t.Id == Item.TestQuestion_Id));
            }
            return PartialView("_GetListByTestPaper", ReturnTestQuestions);
        }

        public ActionResult ListByTestPaper(Guid TestPaperId)
        {
            IList<TestPaper_Question> TestPaper_Questions = this.TestPaper_QuestionDomainService.GetList(new Domain.QueryObject.TestPaper_QuestionCriteria() { TestPaperId = TestPaperId }).OrderBy(t => t.OrderBy).ToList();
            IList<TestQuestion> TestQuestions = this.TestQuestionDomainService.GetList(new Domain.QueryObject.TestQuestionCriteria() { Ids = TestPaper_Questions.Select(t => t.TestQuestion_Id).ToArray() });
            IList<TestQuestion> ReturnTestQuestions = new List<TestQuestion>();
            foreach (var Item in TestPaper_Questions)
            {
                ReturnTestQuestions.Add(TestQuestions.Single(t => t.Id == Item.TestQuestion_Id));
            }
            ViewBag.TestPaperId = TestPaperId;
            return View(ReturnTestQuestions);
        }

        [HttpPost]
        public JsonResult RemoveTestQuestionFromTestPaper(Guid Id, Guid TestPaperId)
        {
            this.TestPaper_QuestionDomainService.Removes(new Domain.QueryObject.TestPaper_QuestionCriteria() { TestQuestionId = Id, TestPaperId = TestPaperId });
            return Json(BC.Core.BaseReturnInfo.Success(string.Empty));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult TestQuestionCreate()
        {
            return View(new AddTestQuestionModel());
        }

        /// <summary>
        /// 保存创建结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestQuestionCreate(AddTestQuestionModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            TestQuestion TestQuestion = ModelInfo.Map<AddTestQuestionModel, TestQuestion>();
            if (!TestQuestion.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(TestQuestion.CustomFormKeycode);
                TestQuestion.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestQuestionFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestQuestionFormItemInfos)
                {
                    TestQuestion.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }

            this.TestQuestionDomainService.AddTestQuestion(TestQuestion);
            ModelInfo.TestQuestionItemModels.Where(t => !t.Answer.IsEmpty() || !t.AttachmentUrl.IsEmpty()).ToList().ForEach(t => 
            {
                TestQuestionItem TestQuestionItem = t.Map<TestQuestionItemModel, TestQuestionItem>();
                TestQuestionItem.TestQuestion = TestQuestion;
                this.TestQuestionItemDomainService.AddTestQuestionItem(TestQuestionItem);
            });
            //this.AttachmentDomainService.RelationAttachments(TestQuestion.Id, ModelInfo.AttachmentIds.ToArray());
            //TestQuestionOperateIndex.InsertIndex(new Guid[] { TestQuestion.Id });
            return this.RefurbishAndClose("TestQuestion");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult TestQuestionEdit(Guid Id)
        {
            TestQuestion TestQuestion = this.TestQuestionDomainService.Single(Id);
            EditTestQuestionModel ModelInfo = TestQuestion.Map<TestQuestion, EditTestQuestionModel>();
            int count = ModelInfo.TestQuestionItemModels.Count;
            for (int i = count; i < 10; i++)
            {
                ModelInfo.TestQuestionItemModels.Add(new TestQuestionItemModel());
            }
            return View(ModelInfo);
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestQuestionEdit(EditTestQuestionModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);
            TestQuestion TestQuestion = this.TestQuestionDomainService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditTestQuestionModel, TestQuestion>(TestQuestion);
            if (!TestQuestion.CustomFormKeycode.IsEmpty())
            {
                CustomForm CustomForm = this.CustomFormDomainService.SingleByKeycode(TestQuestion.CustomFormKeycode);
                TestQuestion.CustomFormKeycode = CustomForm.Key;
                IList<CustomFormItem> TestQuestionFormItemInfos = this.CustomFormItemDomainService.GetCustomFormItems(CustomForm.Id).Where(g => g.Enabled).ToList();
                foreach (var Item in TestQuestionFormItemInfos)
                {
                    TestQuestion.DicExtendedAttributes.Add(Item.Key, new ExtendedAttribute
                    {
                        Key = Item.Key,
                        Name = Item.Name,
                        Value = Request["DicExtendedAttributes_" + Item.Key]
                    });
                }
            }
            this.TestQuestionDomainService.EditTestQuestion(TestQuestion);
            ModelInfo.TestQuestionItemModels.Where(t => !t.Answer.IsEmpty() || !t.AttachmentUrl.IsEmpty()).ToList().ForEach(t =>
            {
                if (!t.Id.IsEmpty())
                {
                    TestQuestionItem TestQuestionItem = TestQuestion.TestQuestionItems.Single(tt => tt.Id == t.Id);
                    t.Map<TestQuestionItemModel, TestQuestionItem>(TestQuestionItem);
                    this.TestQuestionItemDomainService.EditTestQuestionItem(TestQuestionItem);
                }
                else
                {
                    TestQuestionItem TestQuestionItem = t.Map<TestQuestionItemModel, TestQuestionItem>();
                    TestQuestionItem.TestQuestion = TestQuestion;
                    this.TestQuestionItemDomainService.AddTestQuestionItem(TestQuestionItem);
                }
            });
            return this.RefurbishAndClose("TestQuestion");
        }


        // GET: TestQuestion
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取"自动完成"列表
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAutoCompleteTestQuestionNames(string q, int limit)
        {
            return Json(string.Join("\n", this.TestQuestionDomainService.GetList(new Domain.QueryObject.TestQuestionCriteria() { Name = q }, 1, limit).Select(g => g.Title).ToArray()), JsonRequestBehavior.AllowGet);
        } 

    }
}