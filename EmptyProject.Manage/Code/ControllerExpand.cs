using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace EmptyProject.Manage.Code
{
    public static class ControllerExpand
    {
        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static RedirectToRouteResult RefurbishAndClose(this Controller controller, string LoadType = "Default")
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "CloseAndReLoadData", controller = "Helper", LoadType = LoadType }));
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static RedirectToRouteResult ShowMsg(this Controller controller, string Message)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "ShowMsg", controller = "Helper", Message = Message }));
        }

        public static bool VerifyPermission(this Controller controller, string aliasName)
        {
            return true;
            //if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)//判断用户是否登录
            //{
            //    string username = HttpContext.Current.User.Identity.Name;//取得用户名
            //    TrainServices.WEB.UserBAL userBal = new TrainServices.WEB.UserBAL(Membership.ApplicationName, username);
            //    return userBal.ValidateUserPermission(string.Empty, aliasName, string.Empty);
            //}
            //else
            //    return false;
        }
    }
}