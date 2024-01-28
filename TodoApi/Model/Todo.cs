using System.ComponentModel.DataAnnotations;

namespace TodoApi.Model;

public class Todo
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public bool IsComplete { get; set; }
}
