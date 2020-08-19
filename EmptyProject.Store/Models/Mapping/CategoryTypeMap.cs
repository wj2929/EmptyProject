using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class CategoryTypeMap : EntityTypeConfiguration<CategoryType>
    {
        public CategoryTypeMap()
        {
            this.HasRequired(t => t.CustomForm).WithMany().HasForeignKey(t => t.CustomForm_Id);
        }
    }
}
