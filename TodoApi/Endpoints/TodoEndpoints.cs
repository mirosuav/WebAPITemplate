using TodoApi.Extensions;
using TodoApi.Infrastructure;
using TodoApi.Model;
using TodoApi.Tools;

namespace TodoApi.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
    {
        //Return all todos
        group.MapGet("/", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetAllTodos().FreeContext();
            return res.ToHttpOkResult();
        });

        //Return all completed todos
        group.MapGet("/complete", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetCompletedTodos().FreeContext();
            return res.ToHttpOkResult();
        });

        //Returs todo by id
        group.MapGet("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.GetTodo(id).FreeContext();
            return res.ToHttpOkResult();
        });

        //Create new todo
        group.MapPost("/", static async (Todo todo, ITodoService todoService) =>
        {
            var res = await todoService.CreateTodo(todo).FreeContext();
            return res.ToHttpResult(r => TypedResults.Created($"{r.Value!.Id}", r.Value));
        });

        //Updates todo
        group.MapPut("/{id}", static async (int id, Todo todoUpdate, ITodoService todoService) =>
        {
            var res = await todoService.UpdateTodo(id, todoUpdate).FreeContext();
            return res.ToHttpNoContentResult();
        });

        //Delete todo
        group.MapDelete("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.DeleteTodo(id).FreeContext();
            return res.ToHttpNoContentResult();
        });

        return group;
    }

}


