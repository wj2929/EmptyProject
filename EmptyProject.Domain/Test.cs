using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC.DDD.MultiTenancy;
using BC.DDD.Domain;
using System.ComponentModel.DataAnnotations;
namespace EmptyProject.Domain
{
    /// <summary>
    /// 班级
    /// </summary>
    public class Test : EntityWithGuid
    {
        public Test()
        {
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

    }
}
