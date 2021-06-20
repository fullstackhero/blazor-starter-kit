using System.Text.Json;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Options;

namespace BlazorHero.CleanArchitecture.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}