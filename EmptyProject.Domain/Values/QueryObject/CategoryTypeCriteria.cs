using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class CategoryTypeCriteria : IQueryCriteria<CategoryType>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<CategoryType, bool>> Query
        {
            get
            {
                ISpecification<CategoryType> Specification = new Specification<CategoryType>(t => 1 == 1);

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
