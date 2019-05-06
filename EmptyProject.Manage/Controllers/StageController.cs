using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSCX.DomainService.Interface;
using ZSCX.Domain;
using ZSCX.Manage.Code;
using ZSCX.Manage.Models;
using BC.DDD.ObjectMapper;

namespace ZSCX.Manage.Controllers
{
    public class StageController : Controller
    {
        public StageController(IStageInfoDomainService StageInfoDomainService)
        {
            this.StageInfoDomainService = StageInfoDomainService;
        }
        private readonly IStageInfoDomainService StageInfoDomainService;


        // GET: StageInfo
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
                this.StageInfoDomainService.GetStageInfoPaging(
                    new Domain.QueryObject.StageInfoCriteria() { Name = Name }, Page));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult StageInfoCreate()
        {
            return View(new AddStageInfoModel());
        }

        /// <summary>
        /// 保存创建结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StageInfoCreate(AddStageInfoModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);

            StageInfo StageInfo = ModelInfo.Map<AddStageInfoModel, StageInfo>();

            this.StageInfoDomainService.AddStageInfo(StageInfo);
            return this.RefurbishAndClose("StageInfo");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult StageInfoEdit(Guid Id)
        {
            StageInfo StageInfo = this.StageInfoDomainService.Single(Id);
            EditStageInfoModel ModelInfo = StageInfo.Map<StageInfo, EditStageInfoModel>();
            return View(ModelInfo);
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StageInfoEdit(EditStageInfoModel ModelInfo)
        {
            if (!ModelState.IsValid)
                return View(ModelInfo);
            StageInfo StageInfo = this.StageInfoDomainService.Single(ModelInfo.EditId);
            ModelInfo.Map<EditStageInfoModel, StageInfo>(StageInfo);

            this.StageInfoDomainService.EditStageInfo(StageInfo);
            return this.RefurbishAndClose("StageInfo");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteStageInfo(Guid Id)
        {
            this.StageInfoDomainService.Remove(Id);
            return Json("ok");
        }


        /// <summary>
        /// 获取"自动完成"列表
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAutoCompleteStageInfoNames(string q, int limit)
        {
            return Json(string.Join("\n", this.StageInfoDomainService.GetList(new Domain.QueryObject.StageInfoCriteria() { Name = q }, 1, limit).Select(g => g.Name).ToArray()), JsonRequestBehavior.AllowGet);
        } 


    }
}