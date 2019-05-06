using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using System.Text.RegularExpressions;

namespace EmptyProject.Manage.Code.URLPermission
{
    /// <summary>
    /// Summary description for AuthenticationModule
    /// </summary>
    public class AuthenticateModule : IHttpModule
    {
        public AuthenticateModule()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IHttpModule Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(Context_AuthenticateRequest);
        }

        #endregion

        /// <summary>
        /// 登陆用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="persistLogin">保存用户身份到cookie</param>
        /// <returns></returns>
        public static bool AuthenticateUser(string username, string password, bool persistLogin)
        {
            if (Membership.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, persistLogin);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        public static void Logout()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
        }

        static string RemoveAppFromPath(string path, string app)
        {
            path = System.Text.RegularExpressions.Regex.Replace(path, "^" + app, string.Empty, RegexOptions.IgnoreCase);
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            return path;

        }

        static string GetHostUrl()
        {
            string securePort = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            string protocol = securePort == null || securePort == "0" ? "http" : "https";
            string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            string port = serverPort == "80" ? string.Empty : ":" + serverPort;
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            return string.Format("{0}://{1}{2}", protocol, serverName, port);
        }

        /// <summary>
        /// 验证用户身份权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            URLPermissionConfiguration urlPermissionInstance = URLPermissionConfiguration.Instance();

            PathPatternConfiguration excludePathPattern = urlPermissionInstance.ExcludePath;
            PathPatternConfiguration permissionPathPattern = urlPermissionInstance.PermissionPath;

            HttpApplication app = (HttpApplication)sender;

            string requestPath = RemoveAppFromPath(app.Request.Path, app.Request.ApplicationPath);

            if (requestPath == "/" || requestPath == "/default.aspx")
                requestPath = urlPermissionInstance.DefualtPageLocation;

            string redirectUrl = string.Format("{0}/error.aspx?UseCustomIFrameFlag={1}&ReturnUrl={2}",
                GetHostUrl() + app.Request.ApplicationPath.TrimEnd('/'),
                app.Request.RawUrl.ToLower().Contains("usecustomiframeflag"), app.Request.RawUrl);

            //过滤需要进行身份验证的路径
            if (permissionPathPattern.IsMatch(requestPath) || !Regex.IsMatch(requestPath,"\\."))
            {
                //排除（特定）不需要身份验证的路径
                //验证路径中没有"."的路径
                if (!Regex.IsMatch(requestPath, "\\.") && excludePathPattern.IsMatch(requestPath))
                {
                    if(excludePathPattern.IsMatchWithNotExt(requestPath))
                        return;
                }
                else if(excludePathPattern.IsMatch(requestPath))
                    return;

                //if (!excludePathPattern.IsMatch(requestPath))
                //{
                    if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)//登陆用户
                    {
                        //获取用户名
                        string username = app.Context.User.Identity.Name;

                        TrainServices.WEB.UserBAL userBal = new TrainServices.WEB.UserBAL(Membership.ApplicationName,
                            username);

                        //验证访问权限
                        if (!userBal.ValidateUserPermission(requestPath, string.Empty, string.Empty))
                            app.Response.Redirect(redirectUrl);
                    }
                    else
                        app.Response.Redirect(redirectUrl);     //未登录用户
                //}
            }
        }
    }
}
