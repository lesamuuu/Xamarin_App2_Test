using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using toDoCheck.Models;
using toDoCheck.Repositories;

namespace toDoCheck.Services
{
    //public class ToDoItemDBService
    public class ToDoItemDBService<T> where T : new()
    {

        private readonly IRepository<T> _repository;

        public ToDoItemDBService(IRepository<T> repository)
        {
            _repository = repository;
        }
        
        //private void InitializeDatabase(string dbName, bool isTest)
        //{
        //    var folderApp = isTest ? Environment.CurrentDirectory : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    var dbPath = System.IO.Path.Combine(folderApp, dbName);

        //    Connection = new SQLiteAsyncConnection(dbPath);
        //    Connection.CreateTableAsync<ToDoItem>().Wait();
        //}

        public async Task<int> InsertItemAsync(T item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<int> UpdateItemAsync(T item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(T item)
        {
            return await _repository.DeleteAsync(item);
        }

        public async Task<List<T>> GetItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public void Clear()
        {
            _repository.Clear();
        }
    }
}
