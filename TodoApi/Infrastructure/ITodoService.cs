using TodoApi.Model;
using TodoApi.Tools;

namespace TodoApi.Infrastructure;

public interface ITodoService
{
    Task<Result<Todo>> CreateTodo(Todo todo);
    Task<Result<List<Todo>>> GetAllTodos();
    Task<Result<List<Todo>>> GetCompletedTodos();
    Task<Result<Todo>> GetTodo(int id);
    Task<Result<Todo>> UpdateTodo(int id, Todo todoUpdate);
    Task<Result<Todo>> DeleteTodo(int id);
}