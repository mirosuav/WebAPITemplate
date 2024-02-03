using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.Infrastructure;

public sealed class TodoService : ITodoService
{
    private readonly TodoDb db;
    private readonly ILogger<TodoService> logger;

    public TodoService(TodoDb db, ILogger<TodoService> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<Result<List<Todo>>> GetAllTodos()
    {
        return await db.Todos.ToListAsync();
    }

    public async Task<Result<List<Todo>>> GetCompletedTodos()
    {
        return await db.Todos.Where(t => t.IsComplete).ToListAsync();
    }

    public async Task<Result<Todo>> GetTodo(int id)
    {
        return await db.Todos.FindAsync(id) is Todo todo
            ? todo
            : ApiError.NotFound("Todo.NotFound", "No Todo item found with given id");
    }

    public async Task<Result<Todo>> CreateTodo(Todo todo)
    {
        if (todo is null)
            return ApiError.NullValue;

        if (todo.Name is null or [])
            return ApiError.Validation("Todo.InvalidName", "Name property is required");

        if (await db.Todos.SingleOrDefaultAsync(x=>x.Name!.Equals(todo.Name)) is not null)
            return ApiError.Conflict("Todo.NameExists", "Todo with such name already exists");

        db.Add(todo);
        await db.SaveChangesAsync();

        return todo;
    }

    public async Task<Result<Todo>> UpdateTodo(int id, Todo todoUpdate)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null)
            return ApiError.NotFound("Todo.NotFound", "No Todo item found with given id");

        if (todoUpdate is null)
            return todo;

        if (todoUpdate.Name is not null and not [])
            todo.Name = todoUpdate.Name;
        todo.IsComplete = todoUpdate.IsComplete;

        await db.SaveChangesAsync();

        return todo;
    }

    public async Task<Result<Todo>> DeleteTodo(int id)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null)
            return ApiError.NotFound("Todo.NotFound", "No Todo item found with given id");

        db.Todos.Remove(todo);
        await db.SaveChangesAsync();

        return Result<Todo>.Empty;
    }
}
