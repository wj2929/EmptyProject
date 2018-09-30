using BC.DDD.Logging;
using Elmah;
using EmptyProject.DomainService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmptyProject.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 记录异常并格式化返回
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public object RecordError(Exception ex)
        {
            //ExceptionManager.HandleException(ex);

            Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(ex));

            return new
            {
                State = false,
                Message = ex.Message
            };
        }

        //public string GetStudentUserName()
        //{
        //    return "";
        //}

        //public string GetTeacherUserName()
        //{
        //    return "";
        //}

        //public string GetAdminUserName()
        //{
        //    return "Admin";
        //}

        /// <summary>
        /// 获取当前用户所在机构Id
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrganizationId()
        {
            return "Test";
        }
    }
}