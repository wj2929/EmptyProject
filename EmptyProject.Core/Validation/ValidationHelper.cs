using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace EmptyProject.Core.Validation
{
    public static class ValidationHelper
    {
        private static Regex RegPhone = new Regex("^([0-9]{3,4}-[0-9]{6,8})|(1[0-9]{10})$");
        private static Regex RegMobile = new Regex("^[1][0-9]{10}$");
        private static Regex RegNumber = new Regex("^[0-9.]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9.]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegGuid = new Regex("^[a-z0-9]+$");
        private static Regex RegDirectory = new Regex(@"^/[a-zA-Z_0-9]+/", RegexOptions.IgnoreCase);
        private static Regex RegIp = new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
        private static Regex RegZipcode = new Regex("^[0-9]{6}$");
        private static Regex RegTel = new Regex("^[0-9]{3,4}-[0-9]{6,8}$");
        private static Regex RegPassword = new Regex("^[0-9a-zA-Z~!@#$%^&*()_+-={};:?/>.<,]{1,}$");//密码允许字符


        #region 字符串为空
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="InputString">被检测的字符串</param>
        /// <returns></returns>
        public static bool IsEmpty(this string InputString)
        {
            return string.IsNullOrWhiteSpace(InputString);
        }

        public static bool IsNotEmpty(this string InputString)
        {
            return !InputString.IsEmpty();
        }

        /// <summary>
        /// 检测一组字符串中是否存在空的字符串
        /// </summary>
        /// <param name="InputStrings">被检测的字符串组</param>
        /// <returns></returns>
        public static bool IsEmpty(this IList<string> InputStrings)
        {
            if (InputStrings == null || InputStrings.Count == 0)

                foreach (string Item in InputStrings)
                {
                    if (Item.IsEmpty())
                        return true;
                }

            return false;
        }

        /// <summary>
        /// 检测一组字符串中是否存在空的字符串
        /// </summary>
        /// <param name="InputStrings">被检测的字符串组</param>
        /// <returns></returns>
        public static bool IsEmpty(this string[] InputStrings)
        {
            foreach (string Item in InputStrings)
            {
                if (Item.IsEmpty())
                    return true;
            }

            return false;
        } 
        #endregion

        #region Guid
        /// <summary>
        /// 判断Guid是否为空
        /// </summary>
        /// <param name="InputGuid">待判断的Guid</param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid InputGuid)
        {
            return InputGuid == null || InputGuid == Guid.Empty;
        }

        /// <summary>
        /// 判断Guid是否不为空
        /// </summary>
        /// <param name="InputGuid"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this Guid InputGuid)
        {
            return !InputGuid.IsEmpty();
        }

        /// <summary>
        /// 判断Guid是否不为空
        /// </summary>
        /// <param name="InputGuid"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this Guid? InputGuid)
        {
            return !InputGuid.IsEmpty();
        }

        /// <summary>
        /// 判断Guid是否为空
        /// </summary>
        /// <param name="InputGuid"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid? InputGuid)
        {
            if (InputGuid == null) return true;
            return InputGuid.Value.IsEmpty();
        }

        /// <summary>
        /// 判断一组Guid中是否存在空值
        /// </summary>
        /// <param name="InputGuids">待验证的Guid</param>
        /// <returns></returns>
        public static bool IsEmpty(this IList<Guid> InputGuids)
        {
            if (InputGuids==null || InputGuids.Count == 0)
                return true;

            foreach (Guid Item in InputGuids)
            {
                if (Item.IsEmpty())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 从字符串转换Guid
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回Guid.Empty</returns>
        public static Guid GuidByString(this string InputString)
        {
            Guid t = Guid.Empty;
            return Guid.TryParse(InputString, out t) ? t : Guid.Empty;
            
        }

        /// <summary>
        /// 将一组字符串转换为Guid
        /// </summary>
        /// <param name="InputStrings">待转换为Guid的字符串列表</param>
        /// <returns>转换失败的直接抛弃</returns>
        public static IList<Guid> GuidByString(this IList<string> InputStrings)
        {
            IList<Guid> tList = new List<Guid>();

            Guid tGuid = Guid.Empty;
            foreach (string Item in InputStrings)
            {
                if (Guid.TryParse(Item, out tGuid))
                    tList.Add(tGuid);
            }

            return tList;
        }

        /// <summary>
        /// 字符串是否能转换为Guid
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsGuid(this string InputString)
        {
            Guid t = Guid.Empty;
            return Guid.TryParse(InputString, out t);
        }
        #endregion

        #region Int
        /// <summary>
        /// 从字符串转换Int
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static int IntByString(this string InputString)
        {
            return InputString.IntByString(0);
        }

        /// <summary>
        /// 从字符串转换Int
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static int IntByString(this string InputString,int DefaultValue)
        {
            int tInt = 0;
            return int.TryParse(InputString, out tInt) ? tInt : DefaultValue;
        }

        /// <summary>
        /// 将一组字符串转换为Int
        /// </summary>
        /// <param name="InputStrings">待转换为Int的字符串列表</param>
        /// <returns>转换失败的直接抛弃</returns>
        public static IList<int> IntByString(this IList<string> InputStrings)
        {
            IList<int> tList = new List<int>();

            int tInt = 0;
            foreach (string Item in InputStrings)
            {
                if (int.TryParse(Item, out tInt))
                    tList.Add(tInt);
            }

            return tList;
        }

        /// <summary>
        /// 字符串是否能转换为Int
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsInt(this string InputString)
        {
            int tInt = 0;
            return int.TryParse(InputString, out tInt);
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(this string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        #endregion

        #region Decimal
        /// <summary>
        /// 从字符串转换Decimal
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static decimal DecimalByString(this string InputString)
        {
            decimal tDecimal = 0;
            return decimal.TryParse(InputString, out tDecimal) ? tDecimal : 0;
        }

        /// <summary>
        /// 将一组字符串转换为Decimal
        /// </summary>
        /// <param name="InputStrings">待转换为Decimal的字符串列表</param>
        /// <returns>转换失败的直接抛弃</returns>
        public static IList<decimal> DecimalByString(this IList<string> InputStrings)
        {
            IList<decimal> tList = new List<decimal>();

            decimal tDecimal = 0;
            foreach (string Item in InputStrings)
            {
                if (decimal.TryParse(Item, out tDecimal))
                    tList.Add(tDecimal);
            }

            return tList;
        }

        /// <summary>
        /// 字符串是否能转换为Decimal
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string InputString)
        {
            decimal tDecimal = 0;
            return decimal.TryParse(InputString, out tDecimal);
        }
        #endregion

        #region Byte
        /// <summary>
        /// 从字符串转换Byte
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static byte ByteByString(this string InputString)
        {
            byte tByte = 255;
            return byte.TryParse(InputString, out tByte) ? tByte : (byte)255;
        }

        /// <summary>
        /// 字符串是否能转换为Byte
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsByte(this string InputString)
        {
            byte tByte = 255;
            return byte.TryParse(InputString, out tByte);
        }
        #endregion

        #region Bool
        /// <summary>
        /// 从字符串转换Bool
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static bool BoolByString(this string InputString)
        {
            bool tBool = false;
            return bool.TryParse(InputString, out tBool) ? tBool : false;
        }

        public static bool BoolByString(this string InputString,bool DefaultValue)
        {
            bool tBool = DefaultValue;
            if (InputString.IsNotEmpty())
                return bool.TryParse(InputString, out tBool) ? tBool : false;
            else
                return DefaultValue;
        }
        

        /// <summary>
        /// 字符串是否能转换为Bool
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsBool(this string InputString)
        {
            bool tBool = false;
            return bool.TryParse(InputString, out tBool);
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 将字符串转换为日期
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败返回DateTime.Now</returns>
        public static DateTime DateTimeByString(this string InputString)
        {
            DateTime tDateTime = DateTime.Now;
            return DateTime.TryParse(InputString, out tDateTime) ? tDateTime : DateTime.Now;
        }

        /// <summary>
        /// 字符串是否能转换为DateTime
        /// </summary>
        /// <param name="InputString">待检测的字符串</param>
        /// <returns>如果为空返回false</returns>
        public static bool IsDateTime(this string InputString)
        {
            DateTime tDateTime = DateTime.Now;
            return DateTime.TryParse(InputString, out tDateTime);
        }
        #endregion

        #region Long
        /// <summary>
        /// 从字符串转换long
        /// </summary>
        /// <param name="InputString">待转换的字符串</param>
        /// <returns>如果转换失败则返回0</returns>
        public static long LongByString(this string InputString)
        {
            long tLong = 0;
            return long.TryParse(InputString, out tLong) ? tLong : 0;
        }

        /// <summary>
        /// 将一组字符串转换为long
        /// </summary>
        /// <param name="InputStrings">待转换为long的字符串列表</param>
        /// <returns>转换失败的直接抛弃</returns>
        public static IList<long> longByString(this IList<string> InputStrings)
        {
            IList<long> tList = new List<long>();

            long tLong = 0;
            foreach (string Item in InputStrings)
            {
                if (long.TryParse(Item, out tLong))
                    tList.Add(tLong);
            }

            return tList;
        }

        /// <summary>
        /// 字符串是否能转换为long
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <returns></returns>
        public static bool IsLong(this string InputString)
        {
            long tLong = 0;
            return long.TryParse(InputString, out tLong);
        }
        #endregion

        #region 其它
        /// <summary>
        /// 搜索Guid列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="SearchChar"></param>
        /// <returns></returns>
        public static IList<string> SearchGuidList(this IList<Guid> Ids, string SearchChar = ",")
        {
            if (Ids == null || Ids.Count == 0)
                return new List<string>();

            IList<string> tList = new List<string>();
            foreach (Guid Item in Ids)
                tList.Add(Item.ToString().GetSearchString(SearchChar));

            return tList;
        }

        /// <summary>
        /// 在字符串中匹配
        /// </summary>
        /// <param name="TargetString">被匹配的字符串</param>
        /// <param name="Keyword">匹配关键字</param>
        /// <returns></returns>
        public static bool MatchString(this string TargetString, string Keyword, string SearchChar = ",")
        {
            if (TargetString.IsEmpty())
                return false;

            return TargetString.GetSearchString(SearchChar).IndexOf(Keyword.GetSearchString(SearchChar), StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// 返回被指定字符包裹的字符串
        /// </summary>
        /// <param name="InputString">要格式化的字符串</param>
        /// <param name="SearchChar">分隔符</param>
        /// <returns></returns>
        public static string GetSearchString(this string InputString, string SearchChar = ",")
        {
            if (InputString.IsEmpty())
            {
                return "";
            }
            else if (InputString.StartsWith(SearchChar))
            {
                return InputString;
            }
            else
            {
                return String.Format("{0}{1}{0}", SearchChar, InputString);
            }
        }

        /// <summary>
        /// 清理被自定字符包裹的字符串
        /// </summary>
        /// <param name="InputString">要格式化的字符串</param>
        /// <param name="CleanChar">要清理的字符</param>
        /// <returns></returns>
        public static string GetCleanString(this string InputString, string CleanChar = ",")
        {
            if (InputString.IsEmpty())
            {
                return "";
            }
            else if (InputString.StartsWith(CleanChar) && InputString.EndsWith(CleanChar))
            {
                int CharLen = CleanChar.Length;
                int InputStringLen = InputString.Length;
                if (CharLen * 2 == InputStringLen)
                {
                    return "";
                }
                else
                {
                    return InputString.Substring(CharLen, InputStringLen - CharLen * 2);
                }
            }
            else
            {
                return InputString;
            }
        }

        /// <summary>
        /// 返回字符串的字节长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int CnLength(this string str)
        {
            int len = 0;
            for (int i = 0; i < str.Length; i++)
            {
                byte[] byte_len = System.Text.Encoding.Default.GetBytes(str.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;
                else
                    len += 1;
            }
            return len;
        }

        /// <summary>
        /// 拆分一个字符串到Guid列表
        /// </summary>
        /// <param name="InputString">待拆分的字符串</param>
        /// <param name="Separator">分隔符</param>
        /// <returns></returns>
        public static IList<Guid> SplitToGuidList(this string InputString, string Separator = ",")
        {
            if (InputString.IsEmpty() || Separator.IsEmpty())
                return new List<Guid>();

            return Regex.Split(InputString, Separator).ToList().GuidByString();
        }

        /// <summary>
        /// 设置结尾字符
        /// </summary>
        /// <param name="InputString">要设置的字符串</param>
        /// <param name="AddString">结尾字符为</param>
        /// <returns>
        /// 返回处理结果，如果结尾已包括该字符则直接返回，否则附加。
        /// </returns>
        public static string EndChar(this string InputString, char AddChar)
        {
            string _AddChar = AddChar.ToString();

            if (InputString.EndsWith(_AddChar))
                return InputString;

            return InputString + _AddChar;
        }

        /// <summary>
        /// 设置起始字符
        /// </summary>
        /// <param name="InputString">要设置的字符串</param>
        /// <param name="AddChar">起始字符为</param>
        /// <returns>
        /// 返回处理结果，如果开始位置已包括该字符则直接返回，否则附加。
        /// </returns>
        public static string StartChar(this string InputString, char AddChar)
        {
            string _AddChar = AddChar.ToString();

            if (InputString.StartsWith(_AddChar))
                return InputString;

            return _AddChar + InputString;
        }

        /// <summary>
        /// 判断字符串是否为邮编
        /// </summary>
        /// <param name="InputString">输入字符串</param>
        /// <returns>返回是否为邮编</returns>
        public static bool IsZipcode(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return false;
            }
            Match m = RegZipcode.Match(InputString);
            return m.Success;
        }

        /// <summary>
        /// 判断字符串是否为IP
        /// </summary>
        /// <param name="InputString">输入字符串</param>
        /// <returns>返回是否为IP</returns>
        public static bool IsIp(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return false;
            }
            Match m = RegIp.Match(InputString);
            return m.Success;
        }

        /// <summary>
        /// 验证文件名是否合法
        /// </summary>
        /// <param name="InputString">要验证的文件名</param>
        /// <param name="Ext">指定扩展名，用,隔开</param>
        /// <returns></returns>
        public static bool IsFileName(this string InputString, string Ext)
        {
            if (Ext.IsEmpty())
            {
                Regex RegFileName = new Regex(@"^[a-zA-Z_0-9]+[.]{1}[a-zA-Z_0-9]+$", RegexOptions.IgnoreCase);
                return RegFileName.Match(InputString).Success;
            }
            else
            {
                Regex RegFileNameAndExt = new Regex(@"^[a-zA-Z_0-9]+[.]{1}(" + Ext + "){1}$", RegexOptions.IgnoreCase);
                return RegFileNameAndExt.Match(InputString).Success;
            }
        }

        /// <summary>
        /// 判断路径是否合法
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static bool IsDirectory(this string InputString)
        {
            return RegDirectory.Match(InputString).Success;
        }

        /// <summary>
        /// 判断是否为Url（只能判断是否开头是http://）
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static bool IsUrl(this string InputString)
        {
            if (InputString.IsEmpty())
            {
                return false;
            }
            if (InputString.ToLower().StartsWith("http://"))
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
