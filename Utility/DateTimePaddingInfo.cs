using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data
{
    internal class DateTimePaddingInfo
    {
        private PaddingMethod _method;

        public DateTimePaddingInfo(PaddingMethod method)
        {
            _method = method;
        }

        public int GetYear()
        {
            if (_method == PaddingMethod.First)
                return Now.Year;
            else if (_method == PaddingMethod.Last)
                return Now.Year;
            else if (_method == PaddingMethod.Now)
                return Now.Year;
            else
                return -1;
        }

        public int GetMonth()
        {
            if (_method == PaddingMethod.First)
                return 1;
            else if (_method == PaddingMethod.Last)
                return 12;
            else if (_method == PaddingMethod.Now)
                return Now.Month;
            else
                return -1;
        }

        public int GetDay(int year, int month)
        {
            try
            {
                int max = DateTime.DaysInMonth(year, month);

                if (_method == PaddingMethod.First)
                    return 1;
                else if (_method == PaddingMethod.Last)
                    return max;
                else if (_method == PaddingMethod.Now)
                    return Now.Day;
                else
                    return -1;
            }
            catch
            {
                return -1;
            }
        }

        public int GetHour()
        {
            if (_method == PaddingMethod.First)
                return 0;
            else if (_method == PaddingMethod.Last)
                return 23;
            else if (_method == PaddingMethod.Now)
                return Now.Hour;
            else
                return -1;
        }

        public int GetMinute()
        {
            if (_method == PaddingMethod.First)
                return 0;
            else if (_method == PaddingMethod.Last)
                return 59;
            else if (_method == PaddingMethod.Now)
                return Now.Minute;
            else
                return -1;
        }

        public int GetSecond()
        {
            if (_method == PaddingMethod.First)
                return 0;
            else if (_method == PaddingMethod.Last)
                return 59;
            else if (_method == PaddingMethod.Now)
                return Now.Second;
            else
                return -1;
        }

        private static DateTime Now
        {
            get { return DateTime.Now; }
        }

    }
}
