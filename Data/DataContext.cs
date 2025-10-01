using asp.Models;
using asp.Models.category;
using Microsoft.EntityFrameworkCore;

namespace asp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // seed some default categories and todos for dev/test
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General" },
            new Category { Id = 2, Name = "Work" },
            new Category { Id = 3, Name = "Personal" },
            new Category { Id = 4, Name = "Shopping" }
        );

        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem { Id = 1, Title = "Buy milk", Description = "From supermarket", Category = "Shopping" },
            new TodoItem { Id = 2, Title = "Finish report", Description = "Quarterly report", Category = "Work" }
        );
    }
}