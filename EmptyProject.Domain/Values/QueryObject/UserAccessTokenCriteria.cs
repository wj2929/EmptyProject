using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class UserAccessTokenCriteria : IQueryCriteria<UserAccessToken>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<UserAccessToken, bool>> Query
        {
            get
            {
                ISpecification<UserAccessToken> Specification = new Specification<UserAccessToken>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!UserName.IsEmpty())
                    Specification = Specification.And(t => t.UserName == UserName);

                if (UserNames != null && UserNames.Length > 0 && !UserNames[0].IsEmpty())
                    Specification = Specification.And(t => UserNames.Contains(t.UserName));

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public string UserName { get; set; }

        public string[] UserNames { get; set; }


	}
}
