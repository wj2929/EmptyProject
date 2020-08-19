using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class DataStatistics : EntityWithGuid
    {
        public DataStatistics()
        {
             this.CreateDate = DateTime.Now;

        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// StatisticsNum
        /// </summary>
        public int StatisticsNum { get; set; }

        /// <summary>
        /// StatisticsNumByDay
        /// </summary>
        public int StatisticsNumByDay { get; set; }

        /// <summary>
        /// StatisticsNumByWeek
        /// </summary>
        public int StatisticsNumByWeek { get; set; }

        /// <summary>
        /// StatisticsNumByMonth
        /// </summary>
        public int StatisticsNumByMonth { get; set; }

        /// <summary>
        /// StatisticsNumByYear
        /// </summary>
        public int StatisticsNumByYear { get; set; }

        /// <summary>
        /// LastStatisticsNumDate
        /// </summary>
        public DateTime LastStatisticsDate { get; set; }

        public DataStatisticsType DataStatisticsType { get; set; }

        public virtual DataInfo DataInfo { get;set; }
        public Guid DataInfo_Id { get;set; }
    }

    public enum DataStatisticsType
    {
        浏览 = 0,
        投票 = 1
    }
}
