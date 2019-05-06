using System;
using System.Collections.Generic;
using EmptyProject.Core.Config;
using EmptyProject.Core.Collection;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Time
{
    /// <summary>
    /// ShortTime
    /// ����ָ��Сʱ�����ӡ��롣�����ϰ�ʱ�䡢�°�ʱ�䡣
    /// </summary>
    public class ShortTime : IComparable<ShortTime>,IConfigBase<ShortTime>
    {
        #region ���캯��
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

        #region ����
        private int _Hour = 0;
        /// <summary>
        /// Сʱ
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
        /// ��
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
        /// ��
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

        #region ����
        /// <summary>
        /// ����ʱ��ת��Ϊϵͳ����
        /// ��ȡ��ǰʱ�������գ�
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            DateTime now = DateTime.Now;
            return this.GetDateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// ����ʱ��ת��Ϊϵͳ����
        /// ��ָ�������գ�
        /// </summary>
        /// <param name="year">��</param>
        /// <param name="month">��</param>
        /// <param name="day">��</param>
        /// <returns></returns>
        public DateTime GetDateTime(int year, int month, int day)
        {
            DateTime now = DateTime.Now;
            return new DateTime(year, month, day, this.Hour, this.Minute, this.Second);
        }

        /// <summary>
        /// �Ƿ�Ϊָ��ʱ��
        /// </summary>    
        /// <param name="Target">�Ƚ�Ŀ��</param>
        /// <param name="ToleratedErroInSecs">�ݲĬ��Ϊ1�룩</param>
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
        /// ת��Ϊ�ַ���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", this.Hour, this.Minute, this.Second);
        }
        #endregion

        #region IComparable<ShortTime> ��Ա
        /// <summary>
        /// �Ƚ�
        /// </summary>
        /// <param name="Target">�Ƚ�Ŀ��</param>
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
        /// ת��Ϊ�����ļ�
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
        /// �������ļ�ʵ��������
        /// </summary>
        /// <param name="Config">�����ļ�</param>
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
