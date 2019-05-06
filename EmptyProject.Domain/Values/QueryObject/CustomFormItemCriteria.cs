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

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

	}
}
