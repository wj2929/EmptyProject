using System;

namespace EmptyProject.Core.Time
{
    public static class DateTimeHelper
    {
        public enum DateInterval
        {
            Milliseconds, Second, Minute, Hour, Day, Week, Month, Quarter, Year
        }

        public static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Milliseconds:
                    lngDateDiffValue = (long)TS.TotalMilliseconds;
                    break;
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
        /// <summary>
        /// 返回两个时间的间距，用秒来表示
        /// </summary>
        /// <param name="dt1">当前时间</param>
        /// <param name="dt2">对比时间</param>
        /// <returns></returns>
        public static double GetTimeSpan(DateTime dt1, DateTime dt2, TimeType inputtimetype)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1).Duration();
            switch (inputtimetype)
            {
                case TimeType.秒:
                    return ts.TotalSeconds;
                case TimeType.分:
                    return ts.TotalMinutes;
                case TimeType.小时:
                    return ts.TotalHours;
                default:
                    return ts.TotalDays;
            }
        }

        /// <summary>
        /// 是否为指定时间
        /// </summary>
        /// <param name="InputDateTime">参照日期</param>
        /// <param name="TargetDateTime">比较目标</param>
        /// <param name="maxToleranceInSecs">容差（默认为1秒）</param>
        public static bool IsOnTime(this DateTime InputDateTime, DateTime TargetDateTime, int ToleratedErroInSecs = 1)
        {
            return (Math.Abs((TargetDateTime - InputDateTime).TotalMilliseconds) <= (ToleratedErroInSecs * 1000));
        }

        /// <summary>
        /// 是否为制定时间（当前日期作为参照日期）
        /// </summary>
        /// <param name="TargetDateTime">比较目标</param>
        /// <param name="ToleratedErroInSecs">容差（默认为1秒）</param>
        /// <returns></returns>
        public static bool IsOnTime(this DateTime TargetDateTime, int ToleratedErroInSecs = 1)
        {
            return DateTime.Now.IsOnTime(TargetDateTime, ToleratedErroInSecs);
        }

        /// <summary>
        /// 是否为今天（不精确判断）
        /// </summary>
        /// <param name="InputDate"></param>
        /// <returns></returns>
        public static bool ToDay(this DateTime InputDate)
        {
            return DateTime.Now.Day == InputDate.Day;
        }

        /// <summary>
        /// 是否为今年（不精确判断）
        /// </summary>
        /// <param name="InputDate"></param>
        /// <returns></returns>
        public static bool ThisYear(this DateTime InputDate)
        {
            return DateTime.Now.Year == InputDate.Year;
        }

        /// <summary>
        /// 是否为本月（不精确判断）
        /// </summary>
        /// <param name="InputDate"></param>
        /// <returns></returns>
        public static bool ThisMonth(this DateTime InputDate)
        {
            return DateTime.Now.Month == InputDate.Month;
        }

        /// <summary>
        /// 返回日期的字符串拼接形式yyyyMMdd
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static string GetDateString(this DateTime dt1)
        {
            return string.Format("{0:yyyyMMdd}", dt1);
        }

        /// <summary>
        /// 返回当前日期的字符串拼接形式yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateString()
        {
            return DateTime.Now.GetDateString();
        }

        /// <summary>
        /// 返回指定时间日期的完整拼接字符串yyyyMMddhhmmss
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static string GetDateTimeString(this DateTime dt1)
        {
            return string.Format("{0:yyyyMMddhhmmss}", dt1);
        }

        /// <summary>
        /// 返回当前日期的完整拼接字符串yyyyMMddhhmmss
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeString()
        {
            return DateTime.Now.GetDateTimeString();
        }

        /// <summary>
        /// 返回指定时间的完整拼接字符串hhmmss
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static string GetTimeString(this DateTime dt1)
        {
            return string.Format("{0:hhmmss}", dt1);
        }

        /// <summary>
        /// 返回当前时间的完整拼接字符串hhmmss
        /// </summary>
        /// <returns></returns>
        public static string GetTimeString()
        {
            return DateTime.Now.GetTimeString();
        }

        /// <summary>
        /// 计算本周起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateFirstDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }

        /// <summary>
        /// 计算本周结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime CalculateLastDateOfWeek(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }

        /// <summary>
        /// 判断选择的日期是否是本周（根据系统当前时间决定的‘本周’比较而言）
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static bool ThisWeek(this DateTime someDate)
        {
            //得到someDate对应的周一
            DateTime someMon = CalculateFirstDateOfWeek(someDate);
            //得到本周一
            DateTime nowMon = CalculateFirstDateOfWeek(DateTime.Now);

            TimeSpan ts = someMon - nowMon;
            if (ts.Days < 0)
                ts = -ts;//取正
            if (ts.Days >= 7)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得该天的开始时间
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static DateTime ThisDayStartTime(this DateTime someDate)
        {
            return new DateTime(someDate.Year, someDate.Month, someDate.Day, 0, 0, 0);
        }

        /// <summary>
        /// 获得该天的截止时间
        /// </summary>
        /// <param name="someDate"></param>
        /// <returns></returns>
        public static DateTime ThisDayEndTime(this DateTime someDate)
        {
            return new DateTime(someDate.Year, someDate.Month, someDate.Day, 23, 59, 59);
        }

    }

    public enum TimeType
    {
        秒, 分, 小时, 日
    }
}
