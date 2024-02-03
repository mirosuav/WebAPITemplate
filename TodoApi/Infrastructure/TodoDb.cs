﻿using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.Infrastructure;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();


}
