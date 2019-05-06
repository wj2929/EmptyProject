using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmptyProject.Manage.Code
{
    public class DynatreeNode
    {
        public string title { get; set; }
        public bool isFolder { get; set; }
        public string key { get; set; }
        public bool expand { get; set; }
        public bool isLazy { get; set; }
        public IList<DynatreeNode> children = new List<DynatreeNode>();
    }
}