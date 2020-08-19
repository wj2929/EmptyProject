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

                if (Keys != null && Keys.Length > 0 && !Keys[0].IsEmpty())
                    Specification = Specification.And(t => Keys.Contains(t.Key));

                if (!Key.IsEmpty())
                    Specification = Specification.And(t => t.Key == Key);

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public string Key { get; set; }

        public string[] Keys { get; set; }
	}
}
