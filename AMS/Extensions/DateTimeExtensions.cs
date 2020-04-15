using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Extensions
{
    public static class DateExtensions
    {
        public static int Quarter(this DateTime t)
        {
            return (t.Month + 2) / 3;
        }

        public static DateTime QuarterStart(this DateTime t)
        {
            return new DateTime(t.Year, (3 * t.Quarter()) - 2, 1);
        }

        public static DateTime QuarterEnd(this DateTime t)
        {
            return t.QuarterStart().AddMonths(3).AddSeconds(-1);
        }
    }
}