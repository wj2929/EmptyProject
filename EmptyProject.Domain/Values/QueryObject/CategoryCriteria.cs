using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class CategoryCriteria : IQueryCriteria<Category>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<Category, bool>> Query
        {
            get
            {
                ISpecification<Category> Specification = new Specification<Category>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));

                if (!CategoryTypeId.IsEmpty())
                    Specification = Specification.And(t => t.CategoryType_Id == CategoryTypeId);

                Specification = Specification.And(t => t.ParentCategory_Id == ParentCategory_Id);

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public Guid CategoryTypeId { get; set; }

        public Guid? ParentCategory_Id { get; set; }

	}
}
