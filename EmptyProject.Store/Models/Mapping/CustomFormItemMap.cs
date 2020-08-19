using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class CustomFormItemMap : EntityTypeConfiguration<CustomFormItem>
    {
        public CustomFormItemMap()
        {
            this.HasRequired(t => t.CustomForm).WithMany(t => t.Items).HasForeignKey(t => t.CustomForm_Id);

            this.HasOptional(t => t.CustomFormStep).WithMany(t => t.CustomFormItems).HasForeignKey(t => t.CustomFormStep_Id);
        }
    }
}
