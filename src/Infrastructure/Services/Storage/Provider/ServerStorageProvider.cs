using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Storage.Provider;

namespace CleanBlazor.Infrastructure.Services.Storage.Provider;

internal class ServerStorageProvider : IStorageProvider
{
    //private readonly IJSRuntime _jSRuntime;
    //private readonly IJSInProcessRuntime _jSInProcessRuntime;
    private readonly ICurrentUserService _currentUserService;

    //TODO - replace on implementation (added for tests)
    private readonly Dictionary<string, string> _storage = new();

    public ServerStorageProvider(ICurrentUserService currentUserService) => _currentUserService = currentUserService;

    //_jSRuntime = jSRuntime;
    //_jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
    public ValueTask ClearAsync()
        => throw new NotImplementedException(); //_jSRuntime.InvokeVoidAsync("localStorage.clear");

    public ValueTask<string> GetItemAsync(string key)
    {
        //TODO - replace on implementation (added for tests)--
        return ValueTask.FromResult(_storage.TryGetValue(key, out var value) ? value : string.Empty);

        //----------------------------------------------------

        //throw new NotImplementedException();
        //return _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public ValueTask<string> KeyAsync(int index)
        => throw new NotImplementedException(); //_jSRuntime.InvokeAsync<string>("localStorage.key", index);

    public ValueTask<bool> ContainKeyAsync(string key)
        => throw new NotImplementedException(); //_jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);

    public ValueTask<int> LengthAsync()
        => throw new NotImplementedException(); //_jSRuntime.InvokeAsync<int>("eval", "localStorage.length");

    public ValueTask RemoveItemAsync(string key)
        => throw new NotImplementedException(); //_jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

    public ValueTask SetItemAsync(string key, string data)
    {
        //TODO - replace on implementation (added for tests)--
        _storage[key] = data;

        return ValueTask.CompletedTask;
        //----------------------------------------------------

        //throw new NotImplementedException();
        ////_jSRuntime.InvokeVoidAsync("localStorage.setItem", key, data);
    }


    public void Clear() => throw
        //CheckForInProcessRuntime();
        //_jSInProcessRuntime.InvokeVoid("localStorage.clear");
        new NotImplementedException();

    public string GetItem(string key) => throw
        //CheckForInProcessRuntime();
        //return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
        new NotImplementedException();

    public string Key(int index) => throw
        //CheckForInProcessRuntime();
        //return _jSInProcessRuntime.Invoke<string>("localStorage.key", index);
        new NotImplementedException();

    public bool ContainKey(string key) => throw
        //CheckForInProcessRuntime();
        //return _jSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
        new NotImplementedException();

    public int Length() => throw
        //CheckForInProcessRuntime();
        //return _jSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        new NotImplementedException();

    public void RemoveItem(string key) => throw
        //CheckForInProcessRuntime();
        //_jSInProcessRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        new NotImplementedException();

    public void SetItem(string key, string data) => throw
        //CheckForInProcessRuntime();
        //_jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
        new NotImplementedException();

    private void CheckForInProcessRuntime() => throw
        //if (_jSInProcessRuntime == null)
        //    throw new InvalidOperationException("IJSInProcessRuntime not available");
        new NotImplementedException();
}
