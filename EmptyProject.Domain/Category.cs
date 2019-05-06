using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmptyProject.Domain.Values.Category;

namespace EmptyProject.Domain
{
    public class Category : EntityWithGuid
    {
        public Category()
        {
            this.CreateDate = DateTime.Now;
        }

        public Category(string Name, int Order, Guid? ParentId, string Type, string Flag, string ExtendedConfig)
        {
            this.Name = Name;
            this.OrderBy = Order;
            this.Type = Type;
            this.ParentCategory_Id = ParentId;
            this.Flag = Flag;
            this.ExtendedConfig = ExtendedConfig;
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
    }
}
