using SQLite;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using toDoCheck.Models;

namespace toDoCheck.Repositories
{
    public class SQLiteRepository<T> : IRepository<T> where T : new()
    {
        private SQLiteAsyncConnection Connection { get; set; }

        public SQLiteRepository(string dbPath)
        {
            Connection = new SQLiteAsyncConnection(dbPath);
            Connection.CreateTableAsync<T>().Wait();
        }

        public async Task<int> InsertAsync(T item)
        {
            return await Connection.InsertAsync(item);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Connection.Table<T>().ToListAsync();
        }

        public async Task<int> DeleteAsync(T item)
        {
            return await Connection.DeleteAsync(item);
        }

        public async Task<int> UpdateAsync(T item)
        {
            return await Connection.UpdateAsync(item);
        }

        public void Clear()
        {

        }
    }

}

