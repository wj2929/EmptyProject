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
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml;

namespace EmptyProject.Manage.Code.URLPermission
{
    public class XmlSerializerSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XPathNavigator nav = section.CreateNavigator();
            string typename = (string)nav.Evaluate("string(@type)");
            Type t = Type.GetType(typename);
            XmlSerializer ser = new XmlSerializer(t);
            return ser.Deserialize(new XmlNodeReader(section));
        }
    }
}
