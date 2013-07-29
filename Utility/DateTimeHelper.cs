using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace K12.Data
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 標準日期時間格式，例：2007/05/10 14:30:05
        /// </summary>
        public const string StdDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// 標準日期格式，例：2007/05/10
        /// </summary>
        public const string StdDateFormat = "yyyy/MM/dd";
        /// <summary>
        /// 標準時間格式，例：14:30:05
        /// </summary>
        public const string StdTimeFormat = "HH:mm:ss";

        /// <summary>
        /// 判斷DateTime?是否為null以及是否有值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(DateTime? Value)
        {
            return Value == null || !Value.HasValue;
        }

        public static bool IsValidDate(string dateString)
        {
            DateTime dt;
            return DateTime.TryParse(dateString, out dt);
        }

        public static string ToDisplayString(DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToShortDateString();
            else
                return "";
        }

        public static string ToDisplayString(DateTime dt)
        {
            return dt.ToString(StdDateFormat);
        }

        public static string ToDisplayString(string dateTimeString)
        {
            DateTime dt;

            if (DateTime.TryParse(dateTimeString, out dt))
                return dt.ToString(StdDateFormat);
            else
                return string.Empty;
        }

        /// <summary>
        /// 將日期當作是「西元」年處理。
        /// </summary>
        /// <param name="dtString">日期字串。</param>
        public static DateTime? ParseGregorian(string dtString)
        {
            return ParseGregorian(dtString.Trim(), PaddingMethod.None);
        }

        /// <summary>
        /// 將日期當作是「西元」年處理。
        /// </summary>
        /// <param name="dtString">日期字串。 </param>
        /// <param name="method">如果日期缺少的欄位用何種方式補上。</param>
        /// <returns></returns>
        public static DateTime? ParseGregorian(string dtString, PaddingMethod method)
        {
            PatternMatcher matcher = new PatternMatcher(dtString.Trim());

            if (!matcher.IsSuccess)
                return null;
            else
            {
                string assembly;

                if (method == PaddingMethod.None)
                {
                    foreach (Group each in matcher.EachGroup())
                        if (!each.Success) return null;

                    assembly = matcher.GetReassemblyString();
                }
                else
                {
                    int year, month, day, hour, minute, second;
                    DateTimePaddingInfo padding = new DateTimePaddingInfo(method);

                    ExtractInfo(matcher, padding, out year, out month, out day, out hour, out minute, out second);

                    assembly = year + "/" + month + "/" + day + " " + hour + ":" + minute + ":" + second;
                }

                DateTime result;
                if (DateTime.TryParse(assembly, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out result))
                    return result;
                else
                    return null;
            }
        }

        /// <summary>
        /// 將日期當作是「民國」年處理。
        /// </summary>
        /// <param name="dtString">日期字串。</param>
        public static DateTime? ParseTaiwan(string dtString)
        {
            throw new NotImplementedException("未實作。");
        }

        /// <summary>
        /// 將日期當作是「民國」年處理。
        /// </summary>
        /// <param name="dtString">日期字串。</param>
        /// <param name="method">如果日期中缺少的欄位用何種方式補上。</param>
        public static DateTime? ParseTaiwan(string dtString, PaddingMethod method)
        {
            throw new NotImplementedException("未實作。");
        }

        /// <summary>
        /// 使用內鍵的 DateTime 的 TryParse 處理。
        /// </summary>
        /// <param name="dtString">日期字串。</param>
        /// <returns></returns>
        public static DateTime? Parse(string dtString)
        {
            DateTime dt;

            //if (DateTime.TryParse(dtString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dt))

            if (DateTime.TryParse(dtString,out dt))
                return dt;
            else
                return null;
        }

        public static DateTime ParseDirect(string dtstring)
        {
            return DateTime.Parse(dtstring);
        }


        private static void ExtractInfo(PatternMatcher matcher, DateTimePaddingInfo padding, out int year, out int month, out int day, out int hour, out int minute, out int second)
        {
            if (matcher.ContainField(DateTimeField.Year))
                year = matcher.GetFieldValue(DateTimeField.Year);
            else
                year = padding.GetYear();

            if (matcher.ContainField(DateTimeField.Month))
                month = matcher.GetFieldValue(DateTimeField.Month);
            else
                month = padding.GetMonth();

            if (matcher.ContainField(DateTimeField.Day))
                day = matcher.GetFieldValue(DateTimeField.Day);
            else
                day = padding.GetDay(year, month);

            if (matcher.ContainField(DateTimeField.Hour))
                hour = matcher.GetFieldValue(DateTimeField.Hour);
            else
                hour = padding.GetHour();

            if (matcher.ContainField(DateTimeField.Minute))
                minute = matcher.GetFieldValue(DateTimeField.Minute);
            else
                minute = padding.GetMinute();

            if (matcher.ContainField(DateTimeField.Second))
                second = matcher.GetFieldValue(DateTimeField.Second);
            else
                second = padding.GetSecond();
        }
    }
}
