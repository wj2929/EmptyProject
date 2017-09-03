using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmptyProject.Service.Models
{
    public class AddTestModel
    {
        public string Name { get; set; }
    }

    public class DisplayTestModel
    {
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Name { get; set; }
    }
}