using System;
using System.Collections.Generic;
using EmptyProject.Core.Config;
using EmptyProject.Core.Collection;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Time
{
    /// <summary>
    /// ShortTime
    /// 用于指定小时、分钟、秒。比如上班时间、下班时间。
    /// </summary>
    public class ShortTime : IComparable<ShortTime>,IConfigBase<ShortTime>
    {
        #region 构造函数
        public ShortTime() { }
        public ShortTime(int Hour, int Minute, int Second)
        {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
        }

        public ShortTime(DateTime time)
        {
            this.Hour = time.Hour;
            this.Minute = time.Minute;
            this.Second = time.Second;
        }
        #endregion

        #region 属性
        private int _Hour = 0;
        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get { return this._Hour; }
            set
            {
                this._Hour = value;
                this._Hour = this._Hour > 23 ? 23 : this._Hour;
                this._Hour = this._Hour < 0 ? 0 : this._Hour;

            }
        }

        private int _Minute = 0;
        /// <summary>
        /// 分
        /// </summary>
        public int Minute
        {
            get { return this._Minute; }
            set
            {
                this._Minute = value;
                this._Minute = (this._Minute > 59) ? 59 : this._Minute;
                this._Minute = (this._Minute < 0) ? 0 : this._Minute;
            }
        }

        private int _Second = 0;
        /// <summary>
        /// 秒
        /// </summary>
        public int Second
        {
            get { return this._Second; }
            set
            {
                this._Second = value;
                this._Second = (this._Second > 59) ? 59 : this._Second;
                this._Second = (this._Second < 0) ? 0 : this._Second;
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 将短时间转换为系统日期
        /// （取当前时间年月日）
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            DateTime now = DateTime.Now;
            return this.GetDateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// 将短时间转换为系统日期
        /// （指定年月日）
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public DateTime GetDateTime(int year, int month, int day)
        {
            DateTime now = DateTime.Now;
            return new DateTime(year, month, day, this.Hour, this.Minute, this.Second);
        }

        /// <summary>
        /// 是否为指定时间
        /// </summary>    
        /// <param name="Target">比较目标</param>
        /// <param name="ToleratedErroInSecs">容差（默认为1秒）</param>
        public bool IsOnTime(DateTime Target, int ToleratedErroInSecs = 1)
        {
            DateTime dt = this.GetDateTime(Target.Year, Target.Month, Target.Day);

            if (dt.IsOnTime(Target, ToleratedErroInSecs))
                return true;

            if (dt.AddDays(1).IsOnTime(Target, ToleratedErroInSecs))
                return true;

            if (dt.AddDays(-1).IsOnTime(Target, ToleratedErroInSecs))
                return true;

            return false;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", this.Hour, this.Minute, this.Second);
        }
        #endregion

        #region IComparable<ShortTime> 成员
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="Target">比较目标</param>
        /// <returns></returns>
        public int CompareTo(ShortTime Target)
        {
            if ((this.Hour == Target.Hour) && (this.Minute == Target.Minute) && (this.Second == Target.Second))
            {
                return 0;
            }

            #region Compare
            int deltHour = this.Hour - Target.Hour;
            int deltMin = this.Minute - Target.Minute;
            int deltSec = this.Second - Target.Second;
            if (deltHour > 0)
                return 1;

            if (deltHour < 0)
                return -1;

            if (deltMin > 0)
                return 1;

            if (deltMin < 0)
                return -1;

            if (deltSec > 0)
                return 1;

            if (deltSec < 0)
                return -1;

            return 0;
            #endregion
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
            ConfigDic.Add("Hour", this.Hour.ToString());
            ConfigDic.Add("Minute", this.Minute.ToString());
            ConfigDic.Add("Second", this.Second.ToString());
            return ConfigDic.ToConfig("ShortTime");

        }

        /// <summary>
        /// 从配置文件实例化对象
        /// </summary>
        /// <param name="Config">配置文件</param>
        /// <returns></returns>
        public ShortTime FromConfig(string Config)
        {
            if (Config.IsEmpty())
                return null;

            return new ShortTime(
                Config.GetTag("Hour").IntByString(),
                Config.GetTag("Minute").IntByString(),
                Config.GetTag("Second").IntByString()
                );
        } 
        #endregion
    }
}
