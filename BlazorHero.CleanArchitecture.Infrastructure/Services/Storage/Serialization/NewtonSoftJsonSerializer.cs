using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage.Serialization;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.StorageOptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.Serialization
{
    public class NewtonSoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public NewtonSoftJsonSerializer(IOptions<ServerStorageOptions> settings)
        {
            _settings = settings.Value.JsonSerializerSettings;
        }

        public T Deserialize<T>(string text)
            => JsonConvert.DeserializeObject<T>(text, _settings);

        public string Serialize<T>(T obj)
            => JsonConvert.SerializeObject(obj, _settings);
    }
}