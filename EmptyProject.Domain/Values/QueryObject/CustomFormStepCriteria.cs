
using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class CustomFormStepCriteria : IQueryCriteria<CustomFormStep>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<CustomFormStep, bool>> Query
        {
            get
            {
                ISpecification<CustomFormStep> Specification = new Specification<CustomFormStep>(t => 1 == 1);

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
