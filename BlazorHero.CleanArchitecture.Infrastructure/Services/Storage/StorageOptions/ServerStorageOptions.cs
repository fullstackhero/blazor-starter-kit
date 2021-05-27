using System.Text.Json;
using Newtonsoft.Json;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.StorageOptions
{
    public class ServerStorageOptions
    {
        /// <summary>
        /// Options for <see cref="System.Text.Json"/>.
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();

        /// <summary>
        /// Settings for <see cref="Newtonsoft.Json"/>.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}