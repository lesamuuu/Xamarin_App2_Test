using System;
using SQLite;

namespace toDoCheck.Models
{
	public class ToDoItem
	{
		[AutoIncrement, PrimaryKey]
		public int Id { get; set; }
		public bool StatusCompleted { get; set; }
		public string ToDoTask { get; set; }

		public ToDoItem()
		{
		}

        public ToDoItem(string ToDoTask)
        {
			this.ToDoTask = ToDoTask;
			this.StatusCompleted = false;
        }
    }
}

