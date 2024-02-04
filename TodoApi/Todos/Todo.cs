using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Todos;

[Index(nameof(Name), IsUnique = true)]
public class Todo
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public bool IsComplete { get; set; }
}
