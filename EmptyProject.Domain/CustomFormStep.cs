using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class CustomFormStep: EntityWithGuid
    {
        public CustomFormStep()
        {
            this.CreateDate = DateTime.Now;
            this.CustomFormItems = new List<CustomFormItem>();
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public virtual ICollection<CustomFormItem> CustomFormItems { get; set; }

        public virtual CustomForm CustomForm { get; set; }
        public Guid CustomForm_Id { get; set; }
    }
}
