using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

        public async Task<List<T>> Search(string searchedValue)
        {
            List<T> allItems = await GetAllAsync();
            List<T> returnedItems = new List<T>();

            // If seacehedValue is null, we return all items
            if (string.IsNullOrEmpty(searchedValue))
            {
                returnedItems = allItems;
            }
            else
            {
                // Gets all string properties
                var stringProperties = typeof(T).GetProperties()
                                        .Where(p => p.PropertyType == typeof(string))
                                        .ToList();

                searchedValue = searchedValue.ToLower();

                foreach (T item in allItems)
                {
                    foreach (PropertyInfo property in stringProperties)
                    {
                        string value = property.GetValue(item).ToString().ToLower();
                        if (!string.IsNullOrEmpty(value) && value.Contains(searchedValue))
                        {
                            // If property contains searchedValue, returns it
                            returnedItems.Add(item);
                        }
                    }
                }
            }
            return returnedItems;
        }
    }

}

