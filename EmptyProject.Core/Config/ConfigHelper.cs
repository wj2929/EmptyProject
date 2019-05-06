using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Config
{
    /// <summary>
    /// 帮助文件操作助手
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 返回被指定字符包围的Tag数组
        /// </summary>
        /// <param name="InputString">要搜索的字符串</param>
        /// <param name="StartStr"></param>
        /// <param name="EndStr"></param>
        /// <returns></returns>
        public static IList<string> GetTags(this string InputString, string StartStr, string EndStr, bool IncludeTag)
        {
            IList<string> t = new List<string>();
            if (InputString.IsEmpty())
            {
                return t;
            }

            Regex r = new Regex(@"" + StartStr + "(.*?)" + EndStr + "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(InputString);

            if (mc.Count > 0)
            {
                foreach (Match item in mc)
                {
                    if (IncludeTag)
                        t.Add(item.Value);
                    else
                        t.Add(item.Value.Replace(StartStr, "").Replace(EndStr, ""));
                }
            }
            return t;
        }

        /// <summary>
        /// 返回指定标签包围的字符串数组
        /// </summary>
        /// <param name="InputString"></param>
        /// <param name="StartStr"></param>
        /// <param name="EndStr"></param>
        /// <returns></returns>
        public static IList<string> GetTags(this string InputString, string StartStr, string EndStr)
        {
            return InputString.GetTags(StartStr, EndStr, true);
        }

        /// <summary>
        /// 返回指定标签的字符串数组
        /// </summary>
        /// <param name="InputString">输入字符串</param>
        /// <param name="TagName">标签名</param>
        /// <returns></returns>
        public static IList<string> GetTags(this string InputString, string TagName, bool IncludeTag=false)
        {
            return InputString.GetTags("<" + TagName + ">", "</" + TagName + ">", IncludeTag);
        }

        /// <summary>
        /// 返回指定标签内容（Xml格式）
        /// </summary>
        /// <param name="InputString">输入字符串</param>
        /// <param name="TagName">标签名</param>
        /// <returns></returns>
        public static string GetTag(this string InputString, string TagName)
        {
            return InputString.GetTag("<" + TagName + ">", "</" + TagName + ">");
        }

        /// <summary>
        /// 返回指定标签包围的内容
        /// </summary>
        /// <param name="InputString">输入字符串</param>
        /// <param name="StartTag">起始标签</param>
        /// <param name="EndTag">结束标签</param>
        /// <returns></returns>
        public static string GetTag(this string InputString, string StartTag, string EndTag)
        {
            if (InputString.IsEmpty())
                return "";

            Regex r = new Regex(@"" + StartTag + "(.*?)" + EndTag + "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Match m = r.Match(InputString);
            if (m.Success) { return m.Groups[1].ToString(); } else { return ""; }
        }
    }
}
