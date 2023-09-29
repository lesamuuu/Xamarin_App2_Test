using System.Collections.Generic;
using System.Threading.Tasks;
using toDoCheck.Repositories;

namespace toDoCheck.Services
{
    public class ToDoItemDBService<T> where T : new()
    {

        private readonly IRepository<T> _repository;

        public ToDoItemDBService(IRepository<T> repository)
        {
            _repository = repository;
        }

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

        public async Task<List<T>> Search(string searchedValue)
        {
            return await _repository.Search(searchedValue);
        }

        public void Clear()
        {
            _repository.Clear();
        }
    }
}
