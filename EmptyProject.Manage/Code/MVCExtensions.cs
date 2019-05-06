using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class MVCExtensions
    {
        public static MvcHtmlString TrueOrFalseImage(this HtmlHelper helper, bool Value)
        {
            return MvcHtmlString.Create(string.Format("<img src='{0}{1}.png' />",
                new UrlHelper(helper.ViewContext.RequestContext).Content("~/Images/TrueOrFalse/"), Value));
        }



    }
}