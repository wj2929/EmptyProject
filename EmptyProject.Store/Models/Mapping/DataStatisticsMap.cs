using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class DataStatisticsMap : EntityTypeConfiguration<DataStatistics>
    {
        public DataStatisticsMap()
        {
            this.HasRequired(t => t.DataInfo).WithMany(t => t.DataStatistics).HasForeignKey(t => t.DataInfo_Id);
        }
    }
}
