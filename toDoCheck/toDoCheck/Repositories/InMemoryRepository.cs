using System;
using SQLite;
using System.Threading.Tasks;
using toDoCheck.Models;
using System.Collections.Generic;

namespace toDoCheck.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : new()
    {
        private SQLiteAsyncConnection _connection;

        public InMemoryRepository()
        {
            _connection = new SQLiteAsyncConnection(":memory:");
            _connection.CreateTableAsync<T>().Wait();
        }

        public async Task<int> InsertAsync(T item)
        {
            return await _connection.InsertAsync(item);
        }

        public async Task<int> UpdateAsync(T item)
        {
            return await _connection.UpdateAsync(item);
        }

        public async Task<int> DeleteAsync(T item)
        {
            return await _connection.DeleteAsync(item);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _connection.Table<T>().ToListAsync();
        }

        public void Clear()
        {
            _connection.CloseAsync().Wait();
            _connection = new SQLiteAsyncConnection(":memory:");
            _connection.CreateTableAsync<T>().Wait();
        }
    }
}