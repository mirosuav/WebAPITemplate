using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApi.Extensions;
using TodoApi.Infrastructure;
using TodoApi.Model;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
    {
        //Return all todos
        group.MapGet("/", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetAllTodos();
            return res.IsSuccess ? TypedResults.Ok(res.Value) : res.ToProblemResult();
        });

        //Return all completed todos
        group.MapGet("/complete", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetCompletedTodos();
            return res.IsSuccess ? TypedResults.Ok(res.Value) : res.ToProblemResult();
        });

        //Returs todo by id
        group.MapGet("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.GetTodo(id);
            return res.IsSuccess ? TypedResults.Ok(res.Value) : res.ToProblemResult();
        });

        //Create new todo
        group.MapPost("/", static async (Todo todo, ITodoService todoService) =>
        {
            var res = await todoService.CreateTodo(todo);
            return res.IsSuccess ? TypedResults.Created($"{res.Value!.Id}", res.Value) : res.ToProblemResult();
        });

        //Updates todo
        group.MapPut("/{id}", static async (int id, Todo todoUpdate, ITodoService todoService) =>
        {
            var res = await todoService.UpdateTodo(id, todoUpdate);
            return res.IsSuccess ? TypedResults.NoContent() : res.ToProblemResult();
        });

        //Delete todo
        group.MapDelete("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.DeleteTodo(id);
            return res.IsSuccess ? TypedResults.NoContent() : res.ToProblemResult();
        });

        return group;
    }

}


