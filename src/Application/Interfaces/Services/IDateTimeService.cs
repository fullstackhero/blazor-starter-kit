using System;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}