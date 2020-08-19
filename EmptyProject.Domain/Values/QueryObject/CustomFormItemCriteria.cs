using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class CustomFormItemCriteria : IQueryCriteria<CustomFormItem>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<CustomFormItem, bool>> Query
        {
            get
            {
                ISpecification<CustomFormItem> Specification = new Specification<CustomFormItem>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!CustomFormId.IsEmpty())
                    Specification = Specification.And(t => t.CustomForm_Id == CustomFormId);


                if (!CustomFormKey.IsEmpty())
                    Specification = Specification.And(t => t.CustomForm.Key ==CustomFormKey);

                if (OnlyEnabled)
                    Specification = Specification.And(t => t.Enabled);

                if(!Id.IsEmpty())
                    Specification = Specification.And(t => t.Id == Id);

                if (IsLock.HasValue)
                    Specification = Specification.And(t => t.IsLock == IsLock.Value);

                if (!Key.IsEmpty())
                    Specification = Specification.And(t => t.Key == Key);

                if (!Name.IsEmpty())
                    Specification = Specification.And(t => t.Name == Name);

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public Guid CustomFormId { get; set; }

        public string CustomFormKey { get; set; }

        public bool OnlyEnabled { get; set; }

        public bool? IsLock { get; set; }

        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

	}
}
