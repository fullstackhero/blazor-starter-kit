using System;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage
{
    public interface ISyncServerStorageService
    {
        /// <summary>
        /// Clears all data from server storage.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieve the specified data from server storage as a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the server storage slot to use</param>
        /// <returns>The data from the specified <paramref name="key"/> as a <typeparamref name="T"/></returns>
        T GetItem<T>(string key);

        /// <summary>
        /// Retrieve the specified data from server storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>The data associated with the specified <paramref name="key"/> as a <see cref="string"/></returns>
        string GetItemAsString(string key);

        /// <summary>
        /// Return the name of the key at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The name of the key at the specified <paramref name="index"/></returns>
        string Key(int index);

        /// <summary>
        /// Checks if the <paramref name="key"/> exists in server storage, but does not check its value.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>Boolean indicating if the specified <paramref name="key"/> exists</returns>
        bool ContainKey(string key);

        /// <summary>
        /// The number of items stored in server storage.
        /// </summary>
        /// <returns>The number of items stored in server storage</returns>
        int Length();

        /// <summary>
        /// Remove the data with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        void RemoveItem(string key);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in server storage with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The data to be saved</param>
        void SetItem<T>(string key, T data);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in server storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <returns></returns>
        void SetItemAsString(string key, string data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}