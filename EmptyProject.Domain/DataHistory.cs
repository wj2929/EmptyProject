using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class DataHistory : EntityWithGuid
    {
        public DataHistory()
        {
             this.CreateDate = DateTime.Now;

        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        public string Content { get; set; }

        public virtual DataInfo DataInfo { get; set; }
        public Guid DataInfo_Id { get; set; }
    }
}
