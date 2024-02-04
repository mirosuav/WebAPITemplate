using TodoApi.Todos;
using TodoApi.Tools;

namespace TodoApi.Tests.Infrastructure;

public class TodoServiceTests : IDisposable
{
    TodoDb db;
    TodoService sut;
    public TodoServiceTests()
    {
        db = CreateDbContext();
        sut = new TodoService(db);
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
        var result = await sut.GetAllTodos();

        // Assert
        result.Should().BeOfType<Result<List<Todo>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTodos_ReturnsListOfTodos()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await sut.GetAllTodos();

        // Assert
        result.Should().BeOfType<Result<List<Todo>>>();
        result.Value.Should().NotBeNull();
        result.Value.Should().Contain(todos);
    }

    [Fact]
    public async Task GetCompletedTodos_ReturnsListOfCompletedTodos()
    {
        //Arrange
        var todos = (await SeedTodos()).Where(x => x.IsComplete).ToList();

        // Act
        var result = await sut.GetCompletedTodos();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().Contain(todos);

    }

    [Fact]
    public async Task GetTodo_ReturnsTodoIfExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await sut.GetTodo(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.As<Result<Todo>>().Value.Should().BeEquivalentTo(todos[0]);
    }

    [Fact]
    public async Task GetTodo_ReturnsNotFoundIfNotExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await sut.GetTodo(6);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateTodo_Invalid_ReturnsValidationError()
    {
        // Act
        var result = await sut.CreateTodo(new Todo { });

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.Validation);
    }


    [Fact]
    public async Task CreateTodo_DuplicateName_ReturnsValidationError()
    {
        //Arrenge
        var todos = await SeedTodos();

        // Act
        var result = await sut.CreateTodo(new Todo { Name = todos[0].Name });

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task CreateTodo_DuplicateId_IgnoresIdAndSucceed()
    {
        //Arrenge
        var todos = await SeedTodos();

        // Act
        var result = await sut.CreateTodo(new Todo { Id = 1, Name = "Test002" });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task CreateTodo_ReturnsCreated()
    {
        //Arrenge
        var todo = new Todo { Name = "New Todo", IsComplete = true };

        // Act
        var result = await sut.CreateTodo(todo);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(todo);
    }

    [Fact]
    public async Task UpdateTodo_ReturnsNotFoundIfNotExists()
    {
        //Arrange
        var todos = await SeedTodos();

        // Act
        var result = await sut.UpdateTodo(11, new Todo { IsComplete = true });

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task UpdateTodo_ReturnsNoContentOnSuccess()
    {
        //Arrange
        var todos = await SeedTodos();
        var todo = todos[3];
        bool wasComplete = todo.IsComplete;

        // Actvar
        var result = await sut.UpdateTodo(todo.Id, new Todo { IsComplete = !wasComplete });

        // Assert
        result.IsSuccess.Should().BeTrue();

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
        var result = await sut.DeleteTodo(11);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteTodo_ReturnsNoContentOnSuccess()
    {
        //Arrange
        var todos = await SeedTodos();
        var idToDelete = todos[3].Id;

        // Actvar
        var result = await sut.DeleteTodo(idToDelete);

        // Assert
        result.IsSuccess.Should().BeTrue();

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
