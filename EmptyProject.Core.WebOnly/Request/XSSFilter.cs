
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.WebOnly.Request
{
    public static class XSSFilter
    {
        private static string FilterText = "";
        private static Dictionary<string, string> _Model1Dic;
        private static IList<string> _Model2Dic;
        private static Dictionary<string, string> _Model4Dic;
        /// <summary>
        /// 无过滤，仅将转义符替换成正常字符
        /// </summary>
        /// <param name="InputString">传入字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string Model0Filter(this string InputString)
        {
            string ret = InputString.BaseFilter();
            return ret;
        }

        /// <summary>
        /// 按基本字典进行过滤，所有内容均按纯文本显示
        /// </summary>
        /// <param name="InputString">传入字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string Model1Filter(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return string.Empty;
            }
            string ret = InputString.BaseFilter();
            Model1Dic();
            foreach (KeyValuePair<string, string> item in _Model1Dic)
            {
                ret = Regex.Replace(ret, item.Key, item.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }
            return ret;
        }

        /// <summary>   
        /// 清理黑名单中的危险代码，未在黑名单中的Html标签不过滤，正常显示
        /// </summary>   
        /// <param name="InputString">传入字符串</param>   
        /// <returns>过滤后的字符串</returns>   
        public static string Model2Filter(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return string.Empty;
            }
            string ret = InputString.BaseFilter();

            bool found = true;
            Model2Dic();
            while (found)
            {
                var retBefore = ret;
                foreach (string item in _Model2Dic)
                {
                    string pattern = "/";
                    for (int j = 0; j < item.Length; j++)
                    {
                        if (j > 0)
                            pattern = string.Concat(pattern, '(', "(&#[x|X]0{0,8}([9][a][b]);?)?", "|(&#0{0,8}([9][10][13]);?)?", ")?");
                        pattern = string.Concat(pattern, item[j]);
                    }
                    //string replacement = string.Concat(item.Substring(0, 2), "", item.Substring(2));
                    ret = Regex.Replace(ret, pattern, "", RegexOptions.IgnoreCase);
                    if (ret == retBefore)
                    {
                        found = false;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// HtmlEncode
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string Model3Filter(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return string.Empty;
            }
            return InputString.HtmlEncode();
        }

        /// <summary>
        /// 针对Html属性的过滤，比如<img src='' /> 中的src
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string Model4Filter(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return string.Empty;
            }
            string ret = InputString.BaseFilter();
            Model4Dic();
            foreach (KeyValuePair<string, string> item in _Model4Dic)
            {
                ret = Regex.Replace(ret, item.Key, item.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }
            return ret;
        }

        /// <summary>
        /// 基础过滤
        /// </summary>
        /// <param name="InputString">传入字符串</param>
        /// <returns>过滤后的字符串</returns>
        private static string BaseFilter(this string InputString)
        {
            if (InputString.IsEmpty()) return string.Empty;
            string ret = Regex.Replace(InputString, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()~`;:?+/={}[]-_|'\"\\";
            for (int i = 0; i < chars.Length; i++)
            {
                ret = Regex.Replace(ret, string.Concat("(&#[x|X]0{0,}", Convert.ToString((int)chars[i], 16).ToLower(), ";?)"),
                      chars[i].ToString(), RegexOptions.IgnoreCase);
            }
            return ret;
        }

        /// <summary>
        /// 取得Modell4的过滤字典
        /// </summary>
        private static void Model4Dic()
        {
            if (_Model4Dic != null)
            {
                return;
            }
            Dictionary<string, string> tDic = new Dictionary<string, string>();
            tDic.Add("<", "&lt;");
            tDic.Add("%3C", "&lt;");
            tDic.Add("&#x3C;", "&lt;");
            tDic.Add("&#60", "&lt;");
            tDic.Add("PA==", "&lt;");
            tDic.Add(">", "&gt;");
            tDic.Add("%3E", "&gt;");
            tDic.Add("&#x3E;", "&gt;");
            tDic.Add("&#62", "&gt;");
            tDic.Add("Pg==", "&gt;");
            tDic.Add("javascript", "ｊavascript");
            tDic.Add("fromCharCode", "ｆromCharCode");
            tDic.Add("vbscript", "ｖbscript");
            tDic.Add("expression", "ｅxpression");
            tDic.Add("applet", "ａpplet");
            tDic.Add("meta", "ｍeta");
            tDic.Add("xml", "ｘml");
            tDic.Add("link", "ｌink");
            tDic.Add("blink", "ｂlink");
            tDic.Add("style", "ｓtyle");
            tDic.Add("embed", "ｅmbed");
            tDic.Add("script", "ｓcript");
            tDic.Add("object", "ｏbject");
            tDic.Add("iframe", "ｉframe");
            tDic.Add("frame", "ｆrame");
            tDic.Add("frameset", "ｆrameset");
            tDic.Add("isindex", "ｉsindex");
            tDic.Add("ilayer", "ｉlayer");
            tDic.Add("layer", "ｌayer");
            tDic.Add("bgsound", "ｂgsound");
            tDic.Add("title", "ｔitle");
            tDic.Add("base", "ｂase");
            tDic.Add("onabort", "ｏnabort");
            tDic.Add("onactivate", "ｏnactivate");
            tDic.Add("onafterprint", "ｏnafterprint");
            tDic.Add("onafterupdate", "ｏnafterupdate");
            tDic.Add("onbeforeactivate", "ｏnbeforeactivate");
            tDic.Add("onbeforecopy", "ｏnbeforecopy");
            tDic.Add("onbeforecut", "ｏnbeforecut");
            tDic.Add("onbeforedeactivate", "ｏnbeforedeactivate");
            tDic.Add("onbeforeeditfocus", "ｏnbeforeeditfocus");
            tDic.Add("onbeforepaste", "ｏnbeforepaste");
            tDic.Add("onbeforeprint", "ｏnbeforeprint");
            tDic.Add("onbeforeunload", "ｏnbeforeunload");
            tDic.Add("onbeforeupdate", "ｏnbeforeupdate");
            tDic.Add("onblur", "ｏnblur");
            tDic.Add("onbounce", "ｏnbounce");
            tDic.Add("oncellchange", "ｏncellchange");
            tDic.Add("onchange", "ｏnchange");
            tDic.Add("onclick", "ｏnclick");
            tDic.Add("oncontextmenu", "ｏncontextmenu");
            tDic.Add("oncontrolselect", "ｏncontrolselect");
            tDic.Add("oncopy", "ｏncopy");
            tDic.Add("oncut", "ｏncut");
            tDic.Add("ondataavailable", "ｏndataavailable");
            tDic.Add("ondatasetchanged", "ｏndatasetchanged");
            tDic.Add("ondatasetcomplete", "ｏndatasetcomplete");
            tDic.Add("ondblclick", "ｏndblclick");
            tDic.Add("ondeactivate", "ｏndeactivate");
            tDic.Add("ondrag", "ｏndrag");
            tDic.Add("ondragend", "ｏndragend");
            tDic.Add("ondragenter", "ｏndragenter");
            tDic.Add("ondragleave", "ｏndragleave");
            tDic.Add("ondragover", "ｏndragover");
            tDic.Add("ondragstart", "ｏndragstart");
            tDic.Add("ondrop", "ｏndrop");
            tDic.Add("onerror", "ｏnerror");
            tDic.Add("onerrorupdate", "ｏnerrorupdate");
            tDic.Add("onfilterchange", "ｏnfilterchange");
            tDic.Add("onfinish", "ｏnfinish");
            tDic.Add("onfocus", "ｏnfocus");
            tDic.Add("onfocusin", "ｏnfocusin");
            tDic.Add("onfocusout", "ｏnfocusout");
            tDic.Add("onhelp", "ｏnhelp");
            tDic.Add("onkeydown", "ｏnkeydown");
            tDic.Add("onkeypress", "ｏnkeypress");
            tDic.Add("onkeyup", "ｏnkeyup");
            tDic.Add("onlayoutcomplete", "ｏnlayoutcomplete");
            tDic.Add("onload", "ｏnload");
            tDic.Add("onlosecapture", "ｏnlosecapture");
            tDic.Add("onmousedown", "ｏnmousedown");
            tDic.Add("onmouseenter", "ｏnmouseenter");
            tDic.Add("onmouseleave", "ｏnmouseleave");
            tDic.Add("onmousemove", "ｏnmousemove");
            tDic.Add("onmouseout", "ｏnmouseout");
            tDic.Add("onmouseover", "ｏnmouseover");
            tDic.Add("onmouseup", "ｏnmouseup");
            tDic.Add("onmousewheel", "ｏnmousewheel");
            tDic.Add("onmove", "ｏnmove");
            tDic.Add("onmoveend", "ｏnmoveend");
            tDic.Add("onmovestart", "ｏnmovestart");
            tDic.Add("onpaste", "ｏnpaste");
            tDic.Add("onpropertychange", "ｏnpropertychange");
            tDic.Add("onreadystatechange", "ｏnreadystatechange");
            tDic.Add("onreset", "ｏnreset");
            tDic.Add("onresize", "ｏnresize");
            tDic.Add("onresizeend", "ｏnresizeend");
            tDic.Add("onresizestart", "ｏnresizestart");
            tDic.Add("onrowenter", "ｏnrowenter");
            tDic.Add("onrowexit", "ｏnrowexit");
            tDic.Add("onrowsdelete", "ｏnrowsdelete");
            tDic.Add("onrowsinserted", "ｏnrowsinserted");
            tDic.Add("onscroll", "ｏnscroll");
            tDic.Add("onselect", "ｏnselect");
            tDic.Add("onselectionchange", "ｏnselectionchange");
            tDic.Add("onselectstart", "ｏnselectstart");
            tDic.Add("onstart", "ｏnstart");
            tDic.Add("onstop", "ｏnstop");
            tDic.Add("onsubmit", "ｏnsubmit");
            tDic.Add("onunload", "ｏnunload");
            object Lock = new object();
            lock (Lock)
            {
                _Model4Dic = tDic;
            }
        }

        /// <summary>
        /// 取得Model1模式的过滤字典
        /// </summary>
        private static void Model1Dic()
        {
            if (_Model1Dic != null)
            {
                return;
            }
            Dictionary<string, string> tDic = new Dictionary<string, string>();
            tDic.Add("<", "&lt;");
            tDic.Add("%3C", "&lt;");
            tDic.Add("&#x3C;", "&lt;");
            tDic.Add("&#60", "&lt;");
            tDic.Add("PA==", "&lt;");
            tDic.Add(">", "&gt;");
            tDic.Add("%3E", "&gt;");
            tDic.Add("&#x3E;", "&gt;");
            tDic.Add("&#62", "&gt;");
            tDic.Add("Pg==", "&gt;");
            object Lock = new object();
            lock (Lock)
            {
                _Model1Dic = tDic;
            }
        }

        /// <summary>
        /// Model2字典
        /// </summary>
        private static void Model2Dic()
        {
            if (_Model2Dic != null)
            {
                return;
            }
            IList<string> tDic = new List<string>() { 
                "javascript", 
                "fromCharCode",
                "vbscript", 
                "expression",
                "isindex",
                "applet", 
                "meta", 
                "xml", 
                "blink", 
                "link", 
                "style", 
                "script", 
                "embed", 
                "object", 
                "iframe", 
                "frame", 
                "frameset", 
                "ilayer", 
                "layer", 
                "bgsound", 
                "title", 
                "base", 
                "onabort", 
                "onactivate", 
                "onafterprint", 
                "onafterupdate", 
                "onbeforeactivate", 
                "onbeforecopy", 
                "onbeforecut", 
                "onbeforedeactivate",
                "onbeforeeditfocus", 
                "onbeforepaste", 
                "onbeforeprint", 
                "onbeforeunload", 
                "onbeforeupdate", 
                "onblur", 
                "onbounce", 
                "oncellchange", 
                "onchange", 
                "onclick", 
                "oncontextmenu", 
                "oncontrolselect",
                "oncopy", 
                "oncut", 
                "ondataavailable", 
                "ondatasetchanged", 
                "ondatasetcomplete", 
                "ondblclick", 
                "ondeactivate", 
                "ondrag", 
                "ondragend", 
                "ondragenter", 
                "ondragleave", 
                "ondragover", 
                "ondragstart", 
                "ondrop", 
                "onerror", 
                "onerrorupdate", 
                "onfilterchange", 
                "onfinish", 
                "onfocus", 
                "onfocusin", 
                "onfocusout", 
                "onhelp", 
                "onkeydown", 
                "onkeypress", 
                "onkeyup", 
                "onlayoutcomplete",
                "onload", 
                "onlosecapture", 
                "onmousedown",
                "onmouseenter", 
                "onmouseleave", 
                "onmousemove", 
                "onmouseout",
                "onmouseover", 
                "onmouseup", 
                "onmousewheel",
                "onmove", 
                "onmoveend",
                "onmovestart",
                "onpaste", 
                "onpropertychange", 
                "onreadystatechange", 
                "onreset", 
                "onresize", 
                "onresizeend", 
                "onresizestart",
                "onrowenter",
                "onrowexit", 
                "onrowsdelete", 
                "onrowsinserted",
                "onscroll",
                "onselect",
                "onselectionchange",
                "onselectstart", 
                "onstart", 
                "onstop", 
                "onsubmit",
                "onunload" };
            object Lock = new object();
            lock (Lock)
            {
                _Model2Dic = tDic;
            }
        }
    }

}
