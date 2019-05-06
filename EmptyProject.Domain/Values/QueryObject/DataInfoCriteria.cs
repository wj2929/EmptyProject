
using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class DataInfoCriteria : IQueryCriteria<DataInfo>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<DataInfo, bool>> Query
        {
            get
            {
                ISpecification<DataInfo> Specification = new Specification<DataInfo>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if(!CustomForm_Id.IsEmpty())
                    Specification = Specification.And(t => CustomForm_Id == t.CustomForm_Id);

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public Guid CustomForm_Id { get; set; }

	}
}
