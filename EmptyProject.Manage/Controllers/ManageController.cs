using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmptyProject.Web.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        [Authorize]
        public ActionResult Index()
        {
            return Redirect("/APP/index.htm#manage/welcome");
            //return View();
        }

    }
}
