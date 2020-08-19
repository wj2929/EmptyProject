using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class DataInfoMap : EntityTypeConfiguration<DataInfo>
    {
        public DataInfoMap()
        {
            //this.HasRequired(t => t.CustomForm).WithMany().HasForeignKey(t => t.CustomForm_Id);

            //this.HasRequired(t => t.Category).WithMany().HasForeignKey(t => t.Category_Id);

            this.HasOptional(t => t.ParentDataInfo).WithMany(t => t.ChildDataInfos).HasForeignKey(t => t.ParentDataInfo_Id);
        }
    }
}
