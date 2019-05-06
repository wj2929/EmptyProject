using System;
using System.Collections.Generic;
using System.Text;
using EmptyProject.Core.Config;
using EmptyProject.Core.Collection;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Time
{
    /// <summary>
    /// ShortDate 表示年月日。
    /// </summary>
    [Serializable]
    public class ShortDate : IComparable<ShortDate>, IConfigBase<ShortDate>
    {
        #region 构造函数
        public ShortDate()
            : this(DateTime.Now)
        {
        }

        public ShortDate(DateTime dt)
        {
            this.Year = dt.Year;
            this.Month = dt.Month;
            this.Day = dt.Day;
            this.DayOfWeek = dt.DayOfWeek;
        }

        public ShortDate(int Year, int Month, int Day)
            : this(new DateTime(Year, Month, Day))
        {
        }
        #endregion

        #region 属性
        /// <summary>
        /// 周几
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        private int _Month = 1;
        /// <summary>
        /// 月
        /// </summary>
        public int Month
        {
            get { return this._Month; }
            set
            {
                this._Month = value;
                if (this._Month > 12)
                    this._Month = 12;

                if (this._Month < 1)
                    this._Month = 1;
            }
        }

        private int _Day = 1;
        /// <summary>
        /// 日
        /// </summary>
        public int Day
        {
            get { return _Day; }
            set
            {
                DateTime temp = new DateTime(this.Year, this.Month, value);//借助DateTime来验证value的合法性
                this._Day = value;
            }
        }
        #endregion

        #region IComparable<Date> 成员
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="Target">比较目标</param>
        /// <returns></returns>
        public int CompareTo(ShortDate Target)
        {
            if ((this.Year == Target.Year) && (this.Month == Target.Month) && (this.Day == Target.Day))
                return 0;

            #region Compare
            int deltYear = this.Year - Target.Year;
            int deltMon = this.Month - Target.Month;
            int deltDay = this.Day - Target.Day;
            if (deltYear > 0)
                return 1;

            if (deltYear < 0)
                return -1;

            if (deltMon > 0)
                return 1;

            if (deltMon < 0)
                return -1;

            if (deltDay > 0)
                return 1;

            if (deltDay < 0)
                return -1;

            return 0;
            #endregion
        }

        #endregion

        #region 方法
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", this.Year, this.Month, this.Day);
        }

        /// <summary>
        /// 转换为日期对象
        /// </summary>
        /// <param name="Hour">小时</param>
        /// <param name="Minute">分</param>
        /// <param name="Second">秒</param>
        /// <returns></returns>
        public DateTime ToDateTime(int Hour, int Minute, int Second)
        {
            return new DateTime(this.Year, this.Month, this.Day, Hour, Minute, Second);
        }

        /// <summary>
        /// 是否与目标日期相等
        /// </summary>
        /// <param name="Target">比较目标</param>
        /// <returns></returns>
        public bool IsSameDate(DateTime Target)
        {
            return this.CompareTo(new ShortDate(Target)) == 0;
        }

        /// <summary>
        /// 转换为日期对象
        /// （当前时间作为小时、分、秒）
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return this.ToDateTime(0, 0, 0);
        }

        public ShortDate AddDays(int days)
        {
            DateTime dt = new DateTime(this.Year, this.Month, this.Day);
            DateTime newDt = dt.AddDays(days);

            return new ShortDate(newDt);
        }
        #endregion

        #region IConfigBase
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            IDictionary<string, string> ConfigDic = new Dictionary<string, string>();
            ConfigDic.Add("Year", this.Year.ToString());
            ConfigDic.Add("Month", this.Month.ToString());
            ConfigDic.Add("Day", this.Day.ToString());
            return ConfigDic.ToConfig("ShortDate");
        }

        /// <summary>
        /// 从配置文件初始化对象
        /// </summary>
        /// <param name="Config">配置文件</param>
        /// <returns></returns>
        public ShortDate FromConfig(string Config)
        {
            if (Config.IsEmpty())
                return null;

            return new ShortDate(
                    Config.GetTag("Year").IntByString(),
                    Config.GetTag("Month").IntByString(),
                    Config.GetTag("Day").IntByString()
                );
        } 
        #endregion
    }
}
