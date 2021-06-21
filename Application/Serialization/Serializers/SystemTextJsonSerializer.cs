using System.Text.Json;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Serialization.Options;
using Microsoft.Extensions.Options;

namespace BlazorHero.CleanArchitecture.Application.Serialization.Serializers
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<SystemTextJsonOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T Deserialize<T>(string data)
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}