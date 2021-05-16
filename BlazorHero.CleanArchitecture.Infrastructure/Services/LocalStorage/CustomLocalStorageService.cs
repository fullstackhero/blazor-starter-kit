using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.LocalStorage
{
    public class CustomLocalStorageService : ILocalStorageService
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public CustomLocalStorageService(IOptions<LocalStorageOptions> options)
        {
            _jsonOptions = options.Value.JsonSerializerOptions;
        }

        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public ValueTask ClearAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> ContainKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetItemAsStringAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> GetItemAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> KeyAsync(int index)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> LengthAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask RemoveItemAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetItemAsStringAsync(string key, string data)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetItemAsync<T>(string key, T data)
        {
            throw new NotImplementedException();
        }
    }
}
