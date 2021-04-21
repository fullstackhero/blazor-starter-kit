using System;

namespace BlazorHero.CleanArchitecture.DateTimeService.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}