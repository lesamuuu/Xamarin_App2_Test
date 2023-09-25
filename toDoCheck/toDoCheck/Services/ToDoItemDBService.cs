using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using toDoCheck.Models;

namespace toDoCheck.Services
{
    public class ToDoItemDBService : IToDoItemDB
	{

        public SQLiteAsyncConnection Connection { get; set; }

        public ToDoItemDBService() : this("ToDoItems.db3", false) { }

        public ToDoItemDBService(string dbName = "ToDoItems.db3", bool isTest = false)
        {
            InitializeDatabase(dbName, isTest);
        }
        
        private void InitializeDatabase(string dbName, bool isTest)
        {
            var folderApp = isTest ? Environment.CurrentDirectory : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = System.IO.Path.Combine(folderApp, dbName);

            Connection = new SQLiteAsyncConnection(dbPath);
            Connection.CreateTableAsync<ToDoItem>().Wait();
        }

        public async Task<int> InsertItemAsync(ToDoItem item)
        {
            return await Connection.InsertAsync(item);
        }

        public async Task<int> InsertOrUpdateItemAsync(ToDoItem item)
        {
            var existingObject = await Connection.Table<ToDoItem>()
                                             .Where(x => x.Id == item.Id)
                                             .FirstOrDefaultAsync();

            if (existingObject != null)
            {
                return await Connection.UpdateAsync(item);
            }
            else
            {
                return await Connection.InsertAsync(item);
            }
        }

        public async Task<int> ModifyItemAsync(ToDoItem item)
        {
            return await Connection.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(ToDoItem item)
        {
            return await Connection.DeleteAsync(item);
        }

        public async Task<List<ToDoItem>> GetItemsAsync()
        {
            return await Connection.Table<ToDoItem>().ToListAsync();
        }

        
    }
}

