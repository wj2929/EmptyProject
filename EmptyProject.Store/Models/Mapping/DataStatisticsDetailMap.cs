using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class DataStatisticsDetailMap : EntityTypeConfiguration<DataStatisticsDetail>
    {
        public DataStatisticsDetailMap()
        {
            this.HasRequired(t => t.DataInfo).WithMany().HasForeignKey(t => t.DataInfo_Id);
        }
    }
}
