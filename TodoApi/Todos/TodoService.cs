using Microsoft.EntityFrameworkCore;
using TodoApi.Extensions;
using TodoApi.Tools;

namespace TodoApi.Todos;

public sealed class TodoService : ITodoService
{
    private static Error TodoNotFoundError = Error.NotFound("Todo.NotFound", "No Todo item found with given id");
    private static Error TodoInvalidNameError = Error.Validation("Todo.InvalidName", "Name property is required");
    private static Error TodoNameExistsError = Error.Conflict("Todo.NameExists", "Todo with such name already exists");

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
            : TodoNotFoundError;
    }

    public async Task<Result<Todo>> CreateTodo(Todo todo)
    {
        if (todo is null)
            return Error.NullValue;

        if (todo.Name is null or [])
            return TodoInvalidNameError;

        if (await db.Todos.SingleOrDefaultAsync(x => x.Name!.Equals(todo.Name)).FreeContext() is not null)
            return TodoNameExistsError;

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
            return TodoNotFoundError;

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
            return TodoNotFoundError;

        db.Todos.Remove(todo);
        await db.SaveChangesAsync().FreeContext();

        return Result<Todo>.Empty;
    }
}
