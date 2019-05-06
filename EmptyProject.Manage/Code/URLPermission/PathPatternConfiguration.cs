using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Web;
using System.Reflection;
namespace EmptyProject.Manage.Code.URLPermission
{
    /// <summary>
    /// Summary description for PathPatternConfiguration
    /// </summary>
    public class PathPatternConfiguration
    {
        public PathPatternConfiguration()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private string _pattern;
        [XmlAttribute("pattern")]
        public string Pattern
        {
            get { return this._pattern; }
            set { this._pattern = value; }
        }

        private Regex _urlRegex;
        [XmlIgnoreAttribute]
        public Regex UrlRegex
        {
            get
            {
                if (_urlRegex == null)
                {
                    _urlRegex = new Regex(this.Pattern.ToLower(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                return _urlRegex;
            }
        }

        private Regex _urlWithNotExtRegex;
        [XmlIgnoreAttribute]
        public Regex UrlRegexWithNotExt
        {
            get
            {
                if (_urlWithNotExtRegex == null)
                {
                    _urlWithNotExtRegex = new Regex(string.Format("{0}$", this.Pattern.ToLower()), RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                return _urlWithNotExtRegex;
            }
        }

        public bool IsMatch(string url)
        {
            return UrlRegex.IsMatch(url.ToLower());
        }

        public bool IsMatchWithNotExt(string url)
        {
            return UrlRegexWithNotExt.IsMatch(url.ToLower());
        }
    }
}