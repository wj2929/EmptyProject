
using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
    public class UserThirdLoginExtendCriteria : IQueryCriteria<UserThirdLoginExtend>
    {
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<UserThirdLoginExtend, bool>> Query
        {
            get
            {
                ISpecification<UserThirdLoginExtend> Specification = new Specification<UserThirdLoginExtend>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!OpenId.IsEmpty())
                    Specification = Specification.And(t => t.OpenId == OpenId);

                if (!Key.IsEmpty())
                    Specification = Specification.And(t => t.Key == Key);

                if(!BindUserName.IsEmpty())
                    Specification = Specification.And(t => t.BindUserName == BindUserName);

                if (!CreateDate_Begin.IsEmpty())
                {
                    DateTime d_begin_date = CreateDate_Begin.DateTimeByString();
                    Specification = Specification.And(t => t.CreateDate >= d_begin_date);
                }
                if (!CreateDate_End.IsEmpty())
                {
                    DateTime d_end_date = CreateDate_End.DateTimeByString().AddDays(1);
                    Specification = Specification.And(t => t.CreateDate < d_end_date);
                }

                return Specification.Expressions;
            }
        }
        /// <summary>
        /// Ids
        /// </summary>
        public Guid[] Ids { get; set; }

        public string OpenId { get; set; }

        public string Key { get; set; }

        public string BindUserName { get; set; }

        public string CreateDate_Begin { get; set; }

        public string CreateDate_End { get; set; }



    }
}
