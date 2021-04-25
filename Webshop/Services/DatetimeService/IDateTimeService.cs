using System;

namespace Webshop.Services.DatetimeService
{
    public interface IDateTimeService
    {
        DateTime GetCurrentUtc();
    }
}