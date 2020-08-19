using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class UserExtendCriteria : IQueryCriteria<UserExtend>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<UserExtend, bool>> Query
        {
            get
            {
                ISpecification<UserExtend> Specification = new Specification<UserExtend>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!UserName.IsEmpty())
                    Specification = Specification.And(t => UserName == t.UserName);

                if (UserNames != null && UserNames.Length > 0 && !UserNames[0].IsEmpty())
                    Specification = Specification.And(t => UserNames.Contains(t.UserName));

                if(IsLock.HasValue)
                    Specification = Specification.And(t => t.IsLock == IsLock.Value);

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public string UserName { get; set; }

        public string[] UserNames { get; set; }

        public bool? IsLock { get; set; }

	}
}
