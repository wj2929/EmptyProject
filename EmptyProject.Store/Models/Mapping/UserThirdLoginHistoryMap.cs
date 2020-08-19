using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Store.Models.Mapping
{
    public class UserThirdLoginHistoryMap : EntityTypeConfiguration<UserThirdLoginHistory>
    {
        public UserThirdLoginHistoryMap()
        {
            this.HasRequired(t => t.UserThirdLoginExtend).WithMany(t => t.UserThirdLoginHistorys).HasForeignKey(t => t.UserThirdLoginExtend_Id);
        }
    }
}
