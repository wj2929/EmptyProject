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
            this.DataStatistics = new List<DataStatistics>();
            this.DataHistorys = new List<DataHistory>();
            this.ChildDataInfos = new List<DataInfo>();
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
        /// 自定义表单Key
        /// </summary>
        public string CustomFormKeycode { get; set; }


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

        public void SetDicExtendedAttributes(IDictionary<string, ExtendedAttribute> DicExtendedAttributes)
        {
            this.DicExtendedAttributes = DicExtendedAttributes;
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
        /// 数据统计
        /// </summary>
        public virtual ICollection<DataStatistics> DataStatistics { get; set; }

        /// <summary>
        /// 数据历史
        /// </summary>
        public virtual ICollection<DataHistory> DataHistorys { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 上次修改者
        /// </summary>
        public string LastModifyUser { get; set; }

        /// <summary>
        /// 父数据
        /// </summary>
        public virtual DataInfo ParentDataInfo { get; set; }
        public Guid? ParentDataInfo_Id { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Index
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// 子数据列表
        /// </summary>
        public virtual ICollection<DataInfo> ChildDataInfos { get; set; }

    }
}
