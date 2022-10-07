using System;

namespace CitrusDammakuCount
{
    public static class CitrusTime
    {
        public static DateTime UnixTimeToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
        }
    }
}