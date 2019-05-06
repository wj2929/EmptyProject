using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmptyProject.Manage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //管理员登陆
            routes.MapRoute(
                "管理员登陆",
                "Login",
                new { controller = "Account", action = "Login" }
            );

            //管理员登陆
            routes.MapRoute(
                "管理员注销",
                "Logout",
                new { controller = "Account", action = "Logout" }
            );

            //后台管理
            routes.MapRoute(
                "后台管理",
                "Manage",
                new { controller = "Manage", action = "Index", SiteId = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
