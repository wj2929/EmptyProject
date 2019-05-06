using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    /// <summary>
    /// 自定义表单
    /// </summary>
    public class CustomForm : EntityWithGuid
    {
        public CustomForm()
        {
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 所属扩展项
        /// </summary>
        public virtual ICollection<CustomFormItem> Items { get; set; }
    }
}
