using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class UserThirdLoginHistory: EntityWithGuid
    {
        public UserThirdLoginHistory()
        {
            this.CreateDate = DateTime.Now;

        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        public string Key { get; set; }

        public string OpenId { get; set; }

        public string BindUserName { get; set; }

        public virtual UserThirdLoginExtend UserThirdLoginExtend { get; set; }
        public Guid UserThirdLoginExtend_Id { get; set; }
    }
}
