using Microsoft.EntityFrameworkCore;
using TodoApi.Model;
using TodoApi.Tools;

namespace TodoApi.Infrastructure;

public sealed class TodoService : ITodoService
{
    private readonly TodoDb db;

    public TodoService(TodoDb db)
    {
        this.db = db;
    }

    public async Task<Result<List<Todo>>> GetAllTodos()
    {
        return await db.Todos.ToListAsync().FreeContext();
    }

    public async Task<Result<List<Todo>>> GetCompletedTodos()
    {
        return await db.Todos.Where(t => t.IsComplete).ToListAsync().FreeContext();
    }

    public async Task<Result<Todo>> GetTodo(int id)
    {
        return await db.Todos.FindAsync(id).FreeContext() is Todo todo
            ? todo
            : ErrorDetails.NotFound("Todo.NotFound", "No Todo item found with given id");
    }

    public async Task<Result<Todo>> CreateTodo(Todo todo)
    {
        if (todo is null)
            return ErrorDetails.NullValue;

        if (todo.Name is null or [])
            return ErrorDetails.Validation("Todo.InvalidName", "Name property is required");

        if (await db.Todos.SingleOrDefaultAsync(x => x.Name!.Equals(todo.Name)).FreeContext() is not null)
            return ErrorDetails.Conflict("Todo.NameExists", "Todo with such name already exists");

        //Reset ID for new entity
        todo.Id = 0;

        db.Add(todo);
        await db.SaveChangesAsync().FreeContext();

        return todo;
    }

    public async Task<Result<Todo>> UpdateTodo(int id, Todo todoUpdate)
    {
        var todo = await db.Todos.FindAsync(id).FreeContext();
        if (todo is null)
            return ErrorDetails.NotFound("Todo.NotFound", "No Todo item found with given id");

        if (todoUpdate is null)
            return todo;

        if (todoUpdate.Name is not null and not [])
            todo.Name = todoUpdate.Name;
        todo.IsComplete = todoUpdate.IsComplete;

        await db.SaveChangesAsync().FreeContext();

        return todo;
    }

    public async Task<Result<Todo>> DeleteTodo(int id)
    {
        var todo = await db.Todos.FindAsync(id).FreeContext();
        if (todo is null)
            return ErrorDetails.NotFound("Todo.NotFound", "No Todo item found with given id");

        db.Todos.Remove(todo);
        await db.SaveChangesAsync().FreeContext();

        return Result<Todo>.Empty;
    }
}
