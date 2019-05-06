using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class CustomFormCriteria : IQueryCriteria<CustomForm>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<CustomForm, bool>> Query
        {
            get
            {
                ISpecification<CustomForm> Specification = new Specification<CustomForm>(t => 1 == 1);

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
