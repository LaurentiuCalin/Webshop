using System;

namespace Webshop.Services.DatetimeService
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentUtc()
        {
            return DateTime.UtcNow;
        }
    }
}