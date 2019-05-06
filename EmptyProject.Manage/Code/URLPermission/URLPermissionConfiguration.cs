using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EmptyProject.Manage.Code.URLPermission
{
    /// <summary>
    /// Summary description for URLPermissionConfiguration
    /// </summary>
    public class URLPermissionConfiguration
    {
        public URLPermissionConfiguration()
        {

        }

        private string _defaultPageLocation;
        [XmlAttribute("defaultPageLocation")]
        public string DefualtPageLocation
        {
            get { return this._defaultPageLocation; }
            set { this._defaultPageLocation = value; }
        }

        private PathPatternConfiguration _excludePath;
        [XmlElement("ExcludePath")]
        public PathPatternConfiguration ExcludePath
        {
            get { return this._excludePath; }
            set { this._excludePath = value; }
        }

        private PathPatternConfiguration _permissionPath;
        [XmlElement("PermissionPath")]
        public PathPatternConfiguration PermissionPath
        {
            get { return this._permissionPath; }
            set { this._permissionPath = value; }
        }

        private string _fullPageLocation;
        public string FullPageLocation
        {
            get
            {
                if (this._fullPageLocation == null)
                {
                    this._fullPageLocation = HttpContext.Current.Server.MapPath("~/" + DefualtPageLocation);
                }   
                return this._fullPageLocation;
            }
        }

        public static URLPermissionConfiguration Instance()
        {
            return ((URLPermissionConfiguration)ConfigurationManager.GetSection("URLPermissionConfiguration"));
        }
    }
}