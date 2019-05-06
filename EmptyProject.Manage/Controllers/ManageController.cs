using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EmptyProject.Manage.Models;

namespace EmptyProject.Manage.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        [Authorize]
        public ActionResult Index()
        {
            return View(new ManageModels());
        }

        public ActionResult Welcome()
        {
            return View();
        }

    }
}