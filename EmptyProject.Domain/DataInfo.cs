using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC.Core;

namespace EmptyProject.Domain
{
    public class DataInfo : EntityWithGuid
    {
        public DataInfo()
        {
            this.CreateDate = DateTime.Now;
            this.DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();

        }

        /// <summary>
        /// FormId
        /// </summary>
        public Guid CustomForm_Id { get; set; }
        public virtual CustomForm CustomForm { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// ExtendedAttributes
        /// </summary>
        /// <summary>
        /// 扩展属性存储字段
        /// </summary>
        public string ExtendedAttributes
        {
            get
            {
                if (this.DicExtendedAttributes.Count == 0)
                    return "";

                StringBuilder sb = new StringBuilder();
                sb.Append("<ExtendedAttributes>");
                sb.Append("<Items>");
                foreach (KeyValuePair<string, ExtendedAttribute> Item in this.DicExtendedAttributes)
                {
                    sb.Append("<Item>");
                    sb.Append("<Key>");
                    sb.Append(Item.Value.Key);
                    sb.Append("</Key>");
                    sb.Append("<Name>");
                    sb.Append(Item.Value.Name);
                    sb.Append("</Name>");
                    sb.Append("<Value>");
                    sb.Append(Item.Value.Value);
                    sb.Append("</Value>");
                    sb.Append("</Item>");
                }
                sb.Append("</Items>");
                sb.Append("</ExtendedAttributes>");
                return sb.ToString();
            }
            set
            {
                if (value.IsEmpty())
                    return;

                IList<string> _Tags = value.GetTag("Items").GetTags("Item");
                if (_Tags.Count == 0)
                    return;

                foreach (string Item in _Tags)
                {
                    string _Key = Item.GetTag("Key");
                    if (_Key.IsEmpty())
                        continue;

                    this.DicExtendedAttributes.Add(_Key, new ExtendedAttribute()
                    {
                        Key = _Key,
                        Name = Item.GetTag("Name"),
                        Value = Item.GetTag("Value")
                    });
                }
            }
        }
        /// <summary>
        /// 扩展属性列表
        /// </summary>
        public IDictionary<string, ExtendedAttribute> DicExtendedAttributes { get; protected set; }
        public string GetExtendedAttributeValue(string Key)
        {
            if (DicExtendedAttributes.ContainsKey(Key))
                return DicExtendedAttributes[Key].Value;
            else
                return string.Empty;
        }
    }

}
