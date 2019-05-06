using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmptyProject.Manage.Controllers
{
    public class HelperController : Controller
    {

        /// <summary>
        /// 重新加载页面中的指定区域
        /// </summary>
        /// <param name="LoadType"></param>
        /// <returns></returns>
        public ActionResult CloseAndReLoadData(string LoadType = "Default")
        {
            ViewBag.LoadType = LoadType.IsEmpty() ? "Default" : LoadType;
            return View();
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ShowMsg(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }

    }
}
