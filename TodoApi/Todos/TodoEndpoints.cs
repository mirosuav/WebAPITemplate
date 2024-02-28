using TodoApi.Extensions;

namespace TodoApi.Todos;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
    {
        //Return all todos
        group.MapGet("/", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetAllTodos();
            return res.ToHttpOkResult();
        });

        //Return all completed todos
        group.MapGet("/complete", static async (ITodoService todoService) =>
        {
            var res = await todoService.GetCompletedTodos();
            return res.ToHttpOkResult();
        });

        //Returs todo by id
        group.MapGet("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.GetTodo(id);
            return res.ToHttpOkResult();
        });

        //Create new todo
        group.MapPost("/", static async (Todo todo, ITodoService todoService) =>
        {
            var res = await todoService.CreateTodo(todo);
            return res.ToHttpResult(r => TypedResults.Created($"{r.Value!.Id}", r.Value));
        });

        //Updates todo
        group.MapPut("/{id}", static async (int id, Todo todoUpdate, ITodoService todoService) =>
        {
            var res = await todoService.UpdateTodo(id, todoUpdate);
            return res.ToHttpNoContentResult();
        });

        //Delete todo
        group.MapDelete("/{id}", static async (int id, ITodoService todoService) =>
        {
            var res = await todoService.DeleteTodo(id);
            return res.ToHttpNoContentResult();
        });

        return group;
    }

}


