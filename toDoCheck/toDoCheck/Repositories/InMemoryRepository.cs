using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace toDoCheck.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : new()
    {
        private List<T> _memoryDb;

        public InMemoryRepository()
        {
            _memoryDb = new List<T>();
        }

        public async Task<int> InsertAsync(T item)
        {
            _memoryDb.Add(item);
            return await Task.FromResult(1);
        }

        public async Task<int> UpdateAsync(T item)
        {
            // Items have an overriden "Equals" that compares by "Id"
            var existingItem = _memoryDb.Find(itemDb => itemDb.Equals(item));

            if (existingItem != null)
            {
                _memoryDb.Remove(existingItem);
                _memoryDb.Add(item);

                return await Task.FromResult(1);
            }

            return await Task.FromResult(0);
        }

        public async Task<int> DeleteAsync(T item)
        {
            _memoryDb.Remove(item);
            return await Task.FromResult(1);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Task.FromResult(_memoryDb.ToList());
        }

        public void Clear()
        {
            _memoryDb.Clear();
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