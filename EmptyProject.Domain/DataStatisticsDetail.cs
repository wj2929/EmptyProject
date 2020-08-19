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
    public class DataStatisticsDetail : EntityWithGuid
    {
        public DataStatisticsDetail()
        {
             this.CreateDate = DateTime.Now;
             DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();

        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 客户端UA字符串
        /// </summary>
        public string ClientUA { get; set; }


        /// <summary>
        /// 客户端Ip
        /// </summary>
        public string ClientIP { get; set; }


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

        /// <summary>
        /// 数据统计类型
        /// </summary>
        public DataStatisticsType DataStatisticsType { get; set; }

        public virtual DataInfo DataInfo { get; set; }
        public Guid DataInfo_Id { get; set; }
    }

    public class StatisticsInfo
    {
        public int EmptyProjectStatisticsNum { get; set; }
        public int ViewStatisticsNum { get; set; }
    }
}
