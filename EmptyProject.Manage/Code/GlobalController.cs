using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.ObjectMapping;
using EmptyProject.Models;
using AutoMapper;
using BaseCommon.IoC;
using BaseCommon.Core.Config;
using BaseCommon.Core.Object;
using BaseCommon.Core.Net;
using BaseCommon.Core.IO;
using BaseCommon.Core.Validation;
using System.IO;
using System.Web.Security;

namespace EmptyProject.Manage.Code
{
    public partial class GlobalController
    {
        private static IDependencyResolver _IoC;
        /// <summary>
        /// 依赖注入
        /// </summary>
        public static IDependencyResolver IoC
        {
            get
            {
                if (_IoC == null)
                    _IoC = IoCManage.Current;

                return _IoC;
            }
        }

        /// <summary>
        /// 用于缓存接口与实现类的关联
        /// </summary>
        private static ISmallClassBuilder _SmallClassBuilder;
        public static ISmallClassBuilder SmallClassBuilder
        {
            get
            {
                if (_SmallClassBuilder == null)
                    _SmallClassBuilder = new SmallClassBuilder();

                return _SmallClassBuilder;
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

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
        /// 下载html并保存到指定路径
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="SavePath"></param>
        /// <returns></returns>
        public static bool OpenRead(string Url, string SavePath)
        {
            NetHelper NetHelper = new BaseCommon.Core.Net.NetHelper();
            NetHelper.Url = Url;
            string strHTML = NetHelper.Send();
            if (strHTML.IsNotEmpty())
            {
                IOHelper.FileWrite(SavePath, strHTML);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 注入对象映射信息
        /// </summary>
        public static void InjectCustomMap()
        {

        }


        /// <summary>
        /// 写临时CSV文件
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string WriteTempCSVFile(string Content)
        {
            string TempCSVPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp"), "CSV");
            if (!Directory.Exists(TempCSVPath))
                Directory.CreateDirectory(TempCSVPath);
            string TempCSVFile = Path.Combine(TempCSVPath, string.Format("{0}.csv", DateTime.Now.Ticks.ToString()));

            using (StreamWriter sw = new StreamWriter(TempCSVFile, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(Content);
            }
            return TempCSVFile;
        }

    }
}