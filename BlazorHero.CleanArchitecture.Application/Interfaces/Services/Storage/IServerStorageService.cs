using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage
{
    public interface IServerStorageService
    {
        /// <summary>
        /// Clears all data from server storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask ClearAsync();

        /// <summary>
        /// Retrieve the specified data from server storage and deserialize it to the specified type.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the server storage slot to use</param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<T> GetItemAsync<T>(string key);

        /// <summary>
        /// Retrieve the specified data from server storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string> GetItemAsStringAsync(string key);

        /// <summary>
        /// Return the name of the key at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string> KeyAsync(int index);

        /// <summary>
        /// Checks if the <paramref name="key"/> exists in server storage, but does not check its value.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<bool> ContainKeyAsync(string key);

        /// <summary>
        /// The number of items stored in server storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<int> LengthAsync();

        /// <summary>
        /// Remove the data with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask RemoveItemAsync(string key);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in server storage with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The data to be saved</param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask SetItemAsync<T>(string key, T data);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in server storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <returns></returns>
        ValueTask SetItemAsStringAsync(string key, string data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}