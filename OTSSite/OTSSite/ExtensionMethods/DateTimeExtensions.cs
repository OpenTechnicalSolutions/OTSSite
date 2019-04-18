using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSite.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the first day of the week in relation to a supplied DateTime object
        /// </summary>
        /// <param name="dateTime">Reference DateTime object.</param>
        /// <returns>First day of week</returns>
        public static DateTime FirstDayOfWeek(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                return dateTime;

            var weekindex = (int)dateTime.DayOfWeek;
            var sunday = dateTime.Subtract(new TimeSpan(weekindex, 0, 0, 0));
            return sunday;
        }
        /// <summary>
        /// Get the last day of a week in relation to the supplied DateTime object
        /// </summary>
        /// <param name="dateTime">DateTime reference</param>
        /// <returns>Last day of week.</returns>
        public static DateTime LastDayOfWeek(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Friday)
                return dateTime;

            var weekindex = (int)dateTime.DayOfWeek;
            var saturday = dateTime.Add(new TimeSpan(6 - weekindex, 0, 0, 0));
            return saturday;
        }
        /// <summary>
        /// Gets a date formatted for html input date value.
        /// </summary>
        /// <param name="dateTime">DateTime object you need to get date from./param>
        /// <returns>Date formatted for html date value.</returns>
        public static string ApproxDate(this DateTime dateTime)
        {

            var sb = new StringBuilder();
            sb.Append(dateTime.Year).Append('-');

            if (dateTime.Month < 10)
                sb.Append('0').Append(dateTime.Month).Append('-');
            else
                sb.Append(dateTime.Month).Append('-');

            if (dateTime.Day < 10)
                sb.Append('0').Append(dateTime.Day);
            else
                sb.Append(dateTime.Day);

            return sb.ToString();
        }
        /// <summary>
        /// Gets a time formatted for html input time value.
        /// </summary>
        /// <param name="dateTime">DateTime object to get time from.</param>
        /// <returns>Time formatted for html time value.</returns>
        public static string Time24(this DateTime dateTime)
        {
            var hour = dateTime.Hour < 10 ? "0" + dateTime.Hour : dateTime.Hour.ToString();
            var minute = dateTime.Minute < 10 ? "0" + dateTime.Minute : dateTime.Minute.ToString();
            var timeAsString = new StringBuilder()
                .Append(hour)
                .Append(':')
                .Append(minute)
                .ToString();

            return timeAsString;
        }

        public static DateTime RoundToQuarter(this DateTime dateTime)
        {
            var minute = dateTime.Minute;
            if (minute <= 7)
                minute = 0;
            else if (minute > 7 && minute <= 21)
                minute = 15;
            else if (minute > 21 && minute <= 37)
                minute = 30;
            else if (minute > 37 && minute <= 52)
                minute = 45;
            else if (minute > 52)
                minute = 0;

            var dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second, dateTime.Millisecond);
            return dt;
        }
    }
}
