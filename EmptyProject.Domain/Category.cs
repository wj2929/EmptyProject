using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmptyProject.Domain.Values.Category;
using BC.Core;
namespace EmptyProject.Domain
{
    public class Category : EntityWithGuid
    {
        public Category()
        {
            this.CreateDate = DateTime.Now;
            DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
            this.ChildCategorys = new List<Category>();
        }

        public Category(string Name, int Order, Guid? ParentId, string Type, string Flag, string ExtendedConfig)
        {
            this.Name = Name;
            this.OrderBy = Order;
            this.Type = Type;
            this.ParentCategory_Id = ParentId;
            this.Flag = Flag;
            this.ExtendedConfig = ExtendedConfig;
            DicExtendedAttributes = new Dictionary<string, ExtendedAttribute>();
        }


        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }

        public string Index { get; set; }

        public int OrderBy { get; set; }

        public string Type { get; set; }

        public string ExtendedConfig { get; set; }

        public string Flag { get; set; }

        public virtual Category ParentCategory { get; set; }
        public Guid? ParentCategory_Id { get; set; }
        public virtual CategoryType CategoryType { get; set; }
        public Guid CategoryType_Id { get; set; }

        private string _ExtendedHtml;
        public string GetExtendedHtml()
        {

            if (_ExtendedHtml.IsEmpty() && !Type.IsEmpty() && !ExtendedConfig.IsEmpty())
            {
                switch (Type)
                {
                    case "AreaGroup":
                        _ExtendedHtml = new AreaGroupCategoryExtended().FromConfig(ExtendedConfig).ToHtml();
                        break;
                    //case "QuestionType":
                    //    _ExtendedHtml = new QuestionTypeCategoryExtended().FromConfig(ExtendedConfig).ToHtml();
                    //    break;
                    //case "Course":
                    //    _ExtendedHtml = new CourseCategoryExtended().FromConfig(ExtendedConfig).ToHtml();
                    //    break;
                    case "ProvinceCity":
                        _ExtendedHtml = new ProvinceCityCategoryExtended().FromConfig(ExtendedConfig).ToHtml();
                        break;
                    //case "QuestionTemplate":
                    //    _ExtendedHtml = new QuestionTemplateCategoryExtended().FromConfig(ExtendedConfig).ToHtml();
                    //    break;
                    default:
                        _ExtendedHtml = "";
                        break;
                }
                return _ExtendedHtml;
            }
            else
                return string.Empty;
        }

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

        public void SetDicExtendedAttributes(IDictionary<string, ExtendedAttribute> DicExtendedAttributes)
        {
            this.DicExtendedAttributes = DicExtendedAttributes;
        }
        /// <summary>
        /// 子分类列表
        /// </summary>
        public virtual ICollection<Category> ChildCategorys { get; set; } 
    }
}
