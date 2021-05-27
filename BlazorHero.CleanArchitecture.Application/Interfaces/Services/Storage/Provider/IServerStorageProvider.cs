using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage.Provider
{
    public interface IStorageProvider
    {
        void Clear();
        ValueTask ClearAsync();
        bool ContainKey(string key);
        ValueTask<bool> ContainKeyAsync(string key);
        string GetItem(string key);
        ValueTask<string> GetItemAsync(string key);
        string Key(int index);
        ValueTask<string> KeyAsync(int index);
        int Length();
        ValueTask<int> LengthAsync();
        void RemoveItem(string key);
        ValueTask RemoveItemAsync(string key);
        void SetItem(string key, string data);
        ValueTask SetItemAsync(string key, string data);
    }
}