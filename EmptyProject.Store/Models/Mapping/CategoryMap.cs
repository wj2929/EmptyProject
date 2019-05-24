using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.HasRequired(t => t.CategoryType).WithMany().HasForeignKey(t => t.CategoryType_Id);

            this.HasOptional<Category>(t => t.ParentCategory).WithMany();

            this.HasOptional(t => t.ParentCategory).WithMany().HasForeignKey(t => t.ParentCategory_Id);
        }
    }
}
