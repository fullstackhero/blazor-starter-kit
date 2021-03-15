using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using System;

namespace BlazorHero.CleanArchitecture.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}