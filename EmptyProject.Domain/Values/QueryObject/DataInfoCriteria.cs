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

                if (!Name.IsEmpty())
                    Specification = Specification.And(t => t.Name.Contains(Name));

                if (!CustomFormKeycode.IsEmpty())
                    Specification = Specification.And(t => t.CustomFormKeycode == CustomFormKeycode);

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

                Specification = Specification.And(t => t.ParentDataInfo_Id == ParentDataInfo_Id);

                return Specification.Expressions;
            }
        }
        /// <summary>
        /// Ids
        /// </summary>
        public Guid[] Ids { get; set; }

        public string Name { get; set; }

        public string CreateDate_Begin { get; set; }

        public string CreateDate_End { get; set; }

        public string CustomFormKeycode { get; set; }

        public Guid? ParentDataInfo_Id { get; set; }


    }
}
