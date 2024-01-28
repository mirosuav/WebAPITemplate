using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApi.Infrastructure;
using TodoApi.Model;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodos);
        group.MapGet("/complete", GetCompletedTodos);
        group.MapGet("/{id}", GetTodo);
        group.MapPost("/", CreateTodo);
        group.MapPut("/{id}", UpdateTodo);
        group.MapDelete("/{id}", DeleteTodo);

        return group;
    }

    public static async Task<Ok<List<Todo>>> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.ToListAsync());
    }

    public static async Task<Ok<List<Todo>>> GetCompletedTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
    }

    public static async Task<IResult> GetTodo(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id) is Todo todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();
    }

    public static async Task<Created<Todo>> CreateTodo(Todo todo, TodoDb database)
    {
        database.Add(todo);
        await database.SaveChangesAsync();

        return TypedResults.Created($"{todo.Id}", todo);
    }

    public static async Task<IResult> UpdateTodo(int id, Todo todoUpdate, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null)
            return TypedResults.NotFound();

        todo.Name = todoUpdate.Name;
        todo.IsComplete = todoUpdate.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo is null)
            return TypedResults.NotFound();

        db.Todos.Remove(todo);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}


