using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    /// <summary>
    /// 表单扩展属性
    /// </summary>
    public class ExtendedAttribute
    {
        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 扩展属性键值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string Value { get; set; }
    }
}
