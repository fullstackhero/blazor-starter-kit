using System;
using BlazorHero.CleanArchitecture.DateTimeService.Interfaces;

namespace BlazorHero.CleanArchitecture.DateTimeService
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}