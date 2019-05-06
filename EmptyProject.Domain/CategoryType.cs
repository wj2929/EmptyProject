using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class CategoryType : EntityWithGuid
    {
        public CategoryType()
        {
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Keycode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }
    }
}
