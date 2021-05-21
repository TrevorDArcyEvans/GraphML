using System;

namespace ChartJs.Blazor.Common.Time
{
    public static class DateTimeExtensions
    {
        public static double ToJavascript(this DateTime dt)
        {
            return dt.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalMilliseconds;
        }
    }
}
