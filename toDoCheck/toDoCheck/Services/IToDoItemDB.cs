using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using toDoCheck.Models;

namespace toDoCheck.Services
{
	public interface IToDoItemDB
	{
        Task<int> InsertItemAsync(ToDoItem item);
        Task<List<ToDoItem>> GetItemsAsync();
        Task<int> DeleteItemAsync(ToDoItem item);
        Task<int> ModifyItemAsync(ToDoItem item);
        Task<int> InsertOrUpdateItemAsync(ToDoItem item);
    }
}

