using System.Runtime.CompilerServices;

namespace TodoApi.Tests.Endpoints;

public class TodoEndpointsTests : IDisposable
{
    TodoDb db;
    public TodoEndpointsTests()
    {
        db = CreateDbContext();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
        db.Dispose();
    }

    async Task<List<Todo>> SeedTodos()
    {
        var todos = new List<Todo> {
        new Todo { Name = "Test Todo 1", IsComplete = false },
        new Todo { Name = "Test Todo 2", IsComplete = true },
        new Todo { Name = "Test Todo 3", IsComplete = false },
        new Todo { Name = "Test Todo 4", IsComplete = true },
        new Todo { Name = "Test Todo 5", IsComplete = false }
        };
        db.Todos.AddRange(todos);
        await db.SaveChangesAsync();
        return todos;
    }

    [Fact]
    public async Task GetAllTodos_ReturnsEmptyListOfTodos()
    {
        // Act
        var result = await TodoEndpoints.GetAllTodos(db);

        // Assert
        result.Should().BeOfType<Ok<List<Todo>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTodos_ReturnsListOfTodos()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await TodoEndpoints.GetAllTodos(db);

        // Assert
        result.Should().BeOfType<Ok<List<Todo>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().Contain(todos);
    }

    [Fact]
    public async Task GetCompletedTodos_ReturnsListOfCompletedTodos()
    {
        //Arrange
        var todos = (await SeedTodos()).Where(x => x.IsComplete).ToList();

        // Act
        var result = await TodoEndpoints.GetCompletedTodos(db);

        // Assert
        result.Should().BeOfType<Ok<List<Todo>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().Contain(todos);

    }

    [Fact]
    public async Task GetTodo_ReturnsTodoIfExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await TodoEndpoints.GetTodo(1, db);

        // Assert
        result.Should().BeOfType<Ok<Todo>>();
        result.As<Ok<Todo>>().Value.Should().BeEquivalentTo(todos[0]);
    }

    [Fact]
    public async Task GetTodo_ReturnsNotFoundIfNotExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await TodoEndpoints.GetTodo(6, db);

        // Assert
        result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task CreateTodo_ThrowsOnInvalid()
    {
        // Act
        await ((Func<Task>)(async () => await TodoEndpoints.CreateTodo(new Todo { }, db)))
            .Should()
            .ThrowAsync<Exception>();
    }

    [Fact]
    public async Task CreateTodo_ReturnsCreated()
    {
        //Arrenge
        var todo = new Todo { Name = "New Todo", IsComplete = true };

        // Act
        var result = await TodoEndpoints.CreateTodo(todo, db);

        //Assert
        result.Should().BeOfType<Created<Todo>>();
        result.As<Created<Todo>>().Value.Should().Be(todo);
        result.Location.Should().Be($"{todo.Id}");
    }

    [Fact]
    public async Task UpdateTodo_ReturnsNotFoundIfNotExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await TodoEndpoints.UpdateTodo(11, new Todo { IsComplete = true }, db);

        // Assert
        result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task UpdateTodo_ReturnsNoContentOnSuccess()
    {
        //Arrange
        var todos = await SeedTodos();
        var todo = todos[3];
        bool wasComplete = todo.IsComplete;

        // Actvar
        var result = await TodoEndpoints.UpdateTodo(todo.Id, new Todo { IsComplete = !wasComplete }, db);

        // Assert
        result.Should().BeOfType<NoContent>();

        todo = await db.Todos.FindAsync(todo.Id);
        todo.Should().NotBeNull();
        todo!.IsComplete.Should().NotBe(wasComplete);
    }


    [Fact]
    public async Task DeleteTodo_ReturnsNotFoundIfNotExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await TodoEndpoints.DeleteTodo(11, db);

        // Assert
        result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task DeleteTodo_ReturnsNoContentOnSuccess()
    {
        //Arrange
        var todos = await SeedTodos();
        var idToDelete = todos[3].Id;

        // Actvar
        var result = await TodoEndpoints.DeleteTodo(idToDelete, db);

        // Assert
        result.Should().BeOfType<NoContent>();

        (await db.Todos.FindAsync(idToDelete)).Should().BeNull();
    }

    TodoDb CreateDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TodoDb>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var dbContext = new TodoDb(dbContextOptions);
        return dbContext;
    }

}
