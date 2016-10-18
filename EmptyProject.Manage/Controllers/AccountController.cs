using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Core;
using System.Web.Security;
using BaseCommon.Core.Validation;
using EmptyProject.Manage.Models;
using EmptyProject.Manage.Code;

namespace EmptyProject.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return Redirect("/APP/index.htm");
        }

        /// <summary>
        /// 提交登陆
        /// </summary>
        /// <param name="ModelInfo"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel ModelInfo, string ReturnUrl)
        {
            if (GlobalController.AuthenticateUser(ModelInfo.UserName, ModelInfo.Password, true))
            //if (ModelInfo.UserName == "Admin" && ModelInfo.Password == "Admin")
            {
                //FormsAuthentication.SetAuthCookie(ModelInfo.UserName, true);

                if (ReturnUrl.IsEmpty())
                    return RedirectToAction("Index", "Manage");
                else
                    return Redirect(ReturnUrl);
            }
            else
                ModelState.AddModelError("", "用户名或密码错误");

            return View();
        }

        [HttpGet]
        public JsonResult CheckPassword(string Password)
        {
            try
            {
                return Json(new ReturnInfo() { State = GlobalController.ValidateUser(this.User.Identity.Name, Password) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ReturnInfo.Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ModifyPassword(ModifyPasswordModel ModelInfo)
        {
            try
            {
                bool b = GlobalController.ValidateUser(this.User.Identity.Name, ModelInfo.OldPassword);
                if (!b)
                    return Json(ReturnInfo.Error("旧密码错误！"));
                else
                {
                    MembershipUser MembershipUser = Membership.GetUser(this.User.Identity.Name);
                    string NewRandomPassword = MembershipUser.ResetPassword();
                    MembershipUser.ChangePassword(NewRandomPassword, ModelInfo.NewPassword);
                    return Json(ReturnInfo.Success(string.Empty));
                }
            }
            catch (Exception ex)
            {
                return Json(ReturnInfo.Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public JsonResult LoginService(LoginModel ModelInfo)
        {
            try
            {
                if (GlobalController.AuthenticateUser(ModelInfo.UserName, ModelInfo.Password, true))
                {
                    //FormsAuthentication.SetAuthCookie(ModelInfo.UserName, true);

                    return Json(ReturnInfo.Success(string.Empty));
                }
                else
                    return Json(ReturnInfo.Error("用户名或密码错误"));

            }
            catch (Exception ex)
            {
                return Json(ReturnInfo.Error(ex.Message));
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public JsonResult LogoutService()
        {
            try
            {
                FormsAuthentication.SignOut();
                return Json(ReturnInfo.Success(string.Empty));
            }
            catch (Exception ex)
            {
                return Json(ReturnInfo.Error(ex.Message));
            }
        }


        [HttpGet]
        public JsonResult GetUserName()
        {
            try
            {
                if (this.User.Identity.IsAuthenticated)
                    return Json(ReturnInfo.Success(this.User.Identity.Name), JsonRequestBehavior.AllowGet);
                else
                    return Json(ReturnInfo.Error(string.Empty), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ReturnInfo.Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }


    }
}
