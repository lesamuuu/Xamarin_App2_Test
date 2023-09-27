using System;
using SQLite;

namespace toDoCheck.Models
{
	public class Foo
	{
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
		public string Name { get; set; }
    }
}