using System;
using System.Collections.Generic;

namespace ArchLib.Utility
{
    public static class Time
    {
        public static Int64 UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0).Ticks / TimeSpan.TicksPerMillisecond;

        public static Int64 UnixTime(DateTime dt)
        {
            return (dt.Ticks / TimeSpan.TicksPerMillisecond - UnixEpoch);
        }
        public static Int64 UnixTime()
        {
            return UnixTime(DateTime.Now);
        }
    }
}
