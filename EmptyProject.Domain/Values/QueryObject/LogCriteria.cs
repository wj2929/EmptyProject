using BC.DDD.Specification;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace EmptyProject.Domain.QueryObject
{
	public class LogCriteria : IQueryCriteria<Log>
	{ 
        /// <summary>
        /// 查询对象
        /// </summary>
        public Expression<Func<Log, bool>> Query
        {
            get
            {
                ISpecification<Log> Specification = new Specification<Log>(t => 1 == 1);

                if (Ids != null && Ids.Length > 0 && !Ids[0].IsEmpty())
                    Specification = Specification.And(t => Ids.Contains(t.Id));


                if (!UserName.IsEmpty())
                    Specification = Specification.And(t => UserName == t.UserName);

                if(LogType.HasValue)
                    Specification = Specification.And(t => t.LogType == LogType);

                if (!Ip.IsEmpty())
                    Specification = Specification.And(t => t.ClientIp  == Ip);

                if (!Content.IsEmpty())
                    Specification = Specification.And(t => t.Content.Contains(Content));

                if (!Begin_date.IsEmpty())
                {
                    DateTime d_begin_date = Begin_date.DateTimeByString();
                    Specification = Specification.And(t => t.CreateDate >= d_begin_date);
                }
                if (!End_date.IsEmpty())
                {
                    DateTime d_end_date = End_date.DateTimeByString().AddDays(1);
                    Specification = Specification.And(t => t.CreateDate < d_end_date);
                }

                return Specification.Expressions;
            }
        }
		/// <summary>
		/// Ids
		/// </summary>
		public Guid[] Ids { get; set; }

        public string UserName { get; set; }

        public LogType? LogType { get; set; }

        public string Ip { get; set; }

        public string Content { get; set; }

        public string Begin_date { get; set; }

        public string End_date { get; set; }

	}
}
