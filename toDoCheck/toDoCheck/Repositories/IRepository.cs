using System.Collections.Generic;
using System.Threading.Tasks;

namespace toDoCheck.Repositories
{
    public interface IRepository<T>
    {
        Task<int> InsertAsync(T item);
        Task<List<T>> GetAllAsync();
        Task<int> DeleteAsync(T item);
        Task<int> UpdateAsync(T item);
        void Clear();
    }
}

