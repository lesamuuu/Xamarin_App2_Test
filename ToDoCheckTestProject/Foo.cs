using System;
using SQLite;

namespace ToDoCheckTestProject
{
    public class Foo
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is Foo otherItem)
            {
                return this.Id == otherItem.Id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}