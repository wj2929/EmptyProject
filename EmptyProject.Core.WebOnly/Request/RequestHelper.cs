using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.WebOnly.Request
{
    public static class RequestHelper
    {
        /// <summary>
        /// 判断QueryString参数是否为空
        /// </summary>
        /// <param name="inputKey">参数名</param>
        /// <returns></returns>
        public static bool QueryStringIsEmpty(string InputKey)
        {
            if (InputKey.IsEmpty())
            {
                return true;
            }
            else if (HttpContext.Current.Request.QueryString[InputKey] == null)
            {
                return true;
            }
            else if (HttpContext.Current.Request.QueryString[InputKey].Trim().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 一个QueryString是否存在
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static bool QueryStringIsExist(string InputKey)
        {
            if (InputKey.IsEmpty())
            {
                return false;
            }
            if (HttpContext.Current.Request.QueryString[InputKey] == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一组QueryString参数是否为空
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static bool QueryStringIsEmpty(string[] InputKey)
        {
            bool rValue = true;
            foreach (string item in InputKey)
            {
                if (!QueryStringIsEmpty(item))
                {
                    rValue = false;
                }
            }
            return rValue;
        }

        /// <summary>
        /// 判断Request参数是否为空
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static bool RequestIsEmpty(string InputKey)
        {
            if (InputKey.IsEmpty())
            {
                return true;
            }
            else if (HttpContext.Current.Request[InputKey] == null)
            {
                return true;
            }
            else if (HttpContext.Current.Request[InputKey].Trim().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断一组Request参数是否为空
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static bool RequestIsEmpty(string[] InputKey)
        {
            bool rValue = true;
            foreach (string item in InputKey)
            {
                if (!RequestIsEmpty(item))
                {
                    rValue = false;
                }
            }
            return rValue;
        }

        /// <summary>
        /// 判断Form参数是否为空
        /// </summary>
        /// <param name="inputKey">参数名</param>
        /// <returns></returns>
        public static bool FormIsEmpty(string InputKey)
        {
            if (InputKey.IsEmpty())
            {
                return true;
            }
            else if (HttpContext.Current.Request.Form[InputKey] == null)
            {
                return true;
            }
            else if (HttpContext.Current.Request.Form[InputKey].Trim().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 一个Form是否存在
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static bool FormIsExist(string InputKey)
        {
            if (InputKey.IsEmpty())
            {
                return false;
            }
            if (HttpContext.Current.Request.Form[InputKey] == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一组Form参数是否为空
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static bool FormIsEmpty(string[] InputKey)
        {
            bool rValue = true;
            foreach (string item in InputKey)
            {
                if (!FormIsEmpty(item))
                {
                    rValue = false;
                }
            }
            return rValue;
        }

        /// <summary>
        /// 用于替代Request.QueryString
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <param name="Filtering">过滤模式</param>
        /// <returns></returns>
        public static string QueryString(string InputKey, FilterType Filtering)
        {
            if (QueryStringIsEmpty(InputKey))
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.QueryString[InputKey].InputString(Filtering);
        }


        /// <summary>
        /// 用于替代Request
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static string Request(string InputKey)
        {
            return Request(InputKey, FilterType.Model3);
        }

        /// <summary>
        /// 用于替代Request
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <param name="Filtering">过滤模式</param>
        /// <returns></returns>
        public static string Request(string InputKey, FilterType Filtering)
        {
            if (RequestIsEmpty(InputKey))
            {
                return string.Empty;
            }
            return HttpContext.Current.Request[InputKey].InputString(Filtering);
        }


        /// <summary>
        /// 用于替代Request.QueryString
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static string QueryString(string InputKey)
        {
            return QueryString(InputKey, FilterType.Model3);
        }

        /// <summary>
        /// 用于替代Request.Form
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <param name="Filtering">过滤模式</param>
        /// <returns></returns>
        public static string Form(string InputKey, FilterType Filtering)
        {
            if (FormIsEmpty(InputKey))
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Form[InputKey].InputString(Filtering);
        }

        /// <summary>
        /// 用于替代Request.Form
        /// </summary>
        /// <param name="InputKey">参数名</param>
        /// <returns></returns>
        public static string Form(string InputKey)
        {
            return Form(InputKey, FilterType.不过滤);
        }

        /// <summary>
        /// 取得所有Form值
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllKeys()
        {
            return HttpContext.Current.Request.Form.AllKeys;
        }

        /// <summary>
        /// 取得所有QueryString Keys
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllRequestKeys()
        {
            return HttpContext.Current.Request.QueryString.AllKeys;
        }

        /// <summary>
        /// 取得IP（已进行安全检查）
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            //string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (result.IsIp())
            //{
            //    return result;
            //}
            //result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //if (result.IsIp())
            //{
            //    return result;
            //}
            //result = HttpContext.Current.Request.UserHostAddress;
            //if (result.IsIp())
            //{
            //    return result;
            //}
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 取得客户端UA
        /// </summary>
        /// <returns></returns>
        public static string GetClientUA()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// 获取来源地址
        /// </summary>
        /// <returns></returns>
        public static string GetSourceUrl()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            if (result.IsUrl())
            {
                return result;
            }
            result = HttpContext.Current.Request.RawUrl;
            if (result.IsUrl())
            {
                return result;
            }
            return "none";
        }



        /// <summary>
        /// 统一的字符串入口（过滤模式Model3）
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public static string InputString(this string InputStr)
        {
            return InputStr.InputString(FilterType.Model3);
        }

        /// <summary>
        /// 统一的字符串入口，方便以后解决安全问题
        /// </summary>
        /// <param name="InputStr"></param>
        /// <param name="Filtering">过滤模式，提供不同的解决方案</param>
        /// <returns></returns>
        public static string InputString(this string InputStr, FilterType Filtering)
        {
            if (Filtering == FilterType.不过滤)
            {
                return InputStr;
            }
            switch (Filtering)
            {
                case FilterType.Model1:
                    return InputStr.Model1Filter();
                    break;
                case FilterType.Model2:
                    return InputStr.Model2Filter();
                    break;
                case FilterType.Model3:
                    return InputStr.Model3Filter();
                    break;
                case FilterType.Model4:
                    return InputStr.Model4Filter();
                    break;
                default:
                    return InputStr.Model0Filter();
                    break;
            }
        }

        #region 对验证方法的进一步封装
        /// <summary>
        /// 从Form返回DateTime,如果不存在值返回DateTime.Now
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static DateTime DateTimeByForm(string InputKey)
        {
            if (InputKey.IsEmpty() || FormIsEmpty(InputKey))
            {
                return DateTime.Now;
            }
            return Form(InputKey).DateTimeByString();
        }

        /// <summary>
        /// 从Form返回byte,如果不存在值返回0
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static byte ByteByForm(string InputKey)
        {
            if (InputKey.IsEmpty() || FormIsEmpty(InputKey))
            {
                return 255;
            }
            return Form(InputKey).ByteByString();
        }

        /// <summary>
        /// 从QueryString返回byte,如果不存在值返回0
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static byte ByteByQueryString(string InputKey)
        {
            if (InputKey.IsEmpty() || QueryStringIsEmpty(InputKey))
            {
                return 255;
            }
            return QueryString(InputKey).ByteByString();
        }

        /// <summary>
        /// 从Form返回decimal,如果不存在值返回0
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static decimal DecimalByForm(string InputKey)
        {
            if (InputKey.IsEmpty() || FormIsEmpty(InputKey))
            {
                return 0;
            }
            return Form(InputKey).DecimalByString();
        }

        /// <summary>
        /// 从QueryString返回decimal,如果不存在值返回0
        /// </summary>
        /// <param name="InputKey"></param>
        /// <returns></returns>
        public static decimal DecimalByQueryString(string InputKey)
        {
            if (InputKey.IsEmpty() || QueryStringIsEmpty(InputKey))
            {
                return 0;
            }
            return QueryString(InputKey).DecimalByString();
        }

        /// <summary>
        /// 从Form返回Guid，如果不存在值则返回Guid.Empty
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <returns></returns>
        public static Guid GuidByForm(string InputKey)
        {
            if (InputKey.IsEmpty() || FormIsEmpty(InputKey))
            {
                return Guid.Empty;
            }
            return Form(InputKey).GuidByString();
        }

        /// <summary>
        /// 从QueryString返回Guid，如果不存在值则返回Guid.Empty
        /// </summary>
        /// <param name="InputKey">QueryString关键字</param>
        /// <returns></returns>
        public static Guid GuidByQueryString(string InputKey)
        {
            if (InputKey.IsEmpty() || QueryStringIsEmpty(InputKey))
            {
                return Guid.Empty;
            }
            return QueryString(InputKey).GuidByString();
        }

        /// <summary>
        /// 从Form返回Int，如果不存在值则返回0
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <returns></returns>
        public static int IntByForm(string InputKey)
        {
            if (InputKey.IsEmpty() || FormIsEmpty(InputKey))
            {
                return 0;
            }
            return Form(InputKey).IntByString();
        }

        /// <summary>
        /// 从QueryString返回Int，如果不存在值则返回0
        /// </summary>
        /// <param name="InputKey">QueryString关键字</param>
        /// <returns></returns>
        public static int IntByQueryString(string InputKey)
        {
            if (InputKey.IsEmpty() || QueryStringIsEmpty(InputKey))
            {
                return 0;
            }
            return QueryString(InputKey).IntByString();
        }

        /// <summary>
        /// 判断Form中的值是否正确
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <param name="OutPutString">Form中的值</param>
        /// <returns></returns>
        public static bool FormValidate(string InputKey, int Len, out string OutPutString)
        {
            if (FormIsEmpty(InputKey))
            {
                OutPutString = string.Empty;
                return false;
            }
            if (Len > 0)
            {
                if (Form(InputKey).CnLength() > Len)
                {
                    OutPutString = string.Empty;
                    return false;
                }
            }
            OutPutString = Form(InputKey).InputString();
            return true;
        }

        /// <summary>
        /// 判断Form中的值是否正确
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <param name="OutPutString">Form中的值</param>
        /// <returns></returns>
        public static bool QueryStringValidate(string InputKey, out string OutPutString)
        {
            return QueryStringValidate(InputKey, 0, out OutPutString);
        }

        /// <summary>
        /// 判断Form中的值是否正确
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <param name="OutPutString">Form中的值</param>
        /// <returns></returns>
        public static bool QueryStringValidate(string InputKey, int Len, out string OutPutString)
        {
            if (QueryStringIsEmpty(InputKey))
            {
                OutPutString = string.Empty;
                return false;
            }
            if (Len > 0)
            {
                if (QueryString(InputKey).CnLength() > Len)
                {
                    OutPutString = string.Empty;
                    return false;
                }
            }
            OutPutString = QueryString(InputKey).InputString();
            return true;
        }

        /// <summary>
        /// 判断Form中的值是否正确
        /// </summary>
        /// <param name="InputKey">Form关键字</param>
        /// <param name="OutPutString">Form中的值</param>
        /// <returns></returns>
        public static bool FormValidate(string InputKey, out string OutPutString)
        {
            return FormValidate(InputKey, 0, out OutPutString);
        }
        #endregion

        #region 字符串编码
        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string InputData)
        {
            return HttpUtility.HtmlEncode(InputData);
        }

        /// <summary>
        /// 字符串解码
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string InputData)
        {
            return HttpUtility.HtmlDecode(InputData);
        }

        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// 进行Url编码
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string URLEncoding(this string InputString)
        {
            return HttpContext.Current.Server.UrlEncode(InputString);
        }

        #endregion

        /// <summary>
        /// XSS过滤模式
        /// </summary>
        public enum FilterType
        {
            不过滤 = 0, Model0 = 1, Model1 = 2, Model2 = 3, Model3 = 4, Model4 = 5
        }
    }
}
