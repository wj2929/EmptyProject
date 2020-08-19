
using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class UserThirdLoginHistoryCriteria : IQueryCriteria<UserThirdLoginHistory>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<UserThirdLoginHistory, bool>> Query
        {
            get
            {
                ISpecification<UserThirdLoginHistory> Specification = new Specification<UserThirdLoginHistory>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!UserThirdLoginExtendId.IsEmpty())
                    Specification = Specification.And(t => t.UserThirdLoginExtend_Id == UserThirdLoginExtendId);


                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public Guid UserThirdLoginExtendId { get; set; }


    }
}
