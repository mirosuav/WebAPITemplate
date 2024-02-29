using Microsoft.EntityFrameworkCore;

namespace TodoApi.Todos;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos { get; set; }


    public void Seed()
    {
        if (!Todos.Any())
        {
            Todos.Add(new Todo { Name = "Todo 1", IsComplete = false });
            Todos.Add(new Todo { Name = "Todo 2", IsComplete = false });
            Todos.Add(new Todo { Name = "Todo 3", IsComplete = true });
            SaveChanges();
        }
    }

}


