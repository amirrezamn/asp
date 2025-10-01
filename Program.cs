using asp.Data;
using Microsoft.EntityFrameworkCore;
using asp.Models;
using asp.Models.category;


// Builder
var builder = WebApplication.CreateBuilder(args);

// SQLite
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "todo.db");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build app
var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

// Endpoints

// Redirect root
app.MapGet("/", () => Results.Redirect("/swagger"));

// Categories
app.MapGet("/api/categories", async (DataContext db) =>
    await db.Categories.OrderBy(c => c.Name).ToListAsync());

app.MapPost("/api/categories", async (DataContext db, Category cat) =>
{
    if (string.IsNullOrWhiteSpace(cat.Name)) return Results.BadRequest("Name required");
    if (await db.Categories.AnyAsync(c => c.Name.ToLower() == cat.Name.ToLower()))
        return Results.Conflict("Category exists");

    db.Categories.Add(cat);
    await db.SaveChangesAsync();
    return Results.Created($"/api/categories/{cat.Id}", cat);
});

// Todos
app.MapGet("/api/todos", async (DataContext db, string? q, string? category) =>
{
    var query = db.Todos.AsQueryable();
    if (!string.IsNullOrWhiteSpace(q))
    {
        var lower = q.Trim().ToLower();
        query = query.Where(t => t.Title.ToLower().Contains(lower) ||
                                 (t.Description != null && t.Description.ToLower().Contains(lower)));
    }
    if (!string.IsNullOrWhiteSpace(category))
    {
        query = query.Where(t => t.Category == category);
    }
    return Results.Ok(await query.ToListAsync());
});

app.MapGet("/api/todos/{id:long}", async (DataContext db, long id) =>
{
    var todo = await db.Todos.FindAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/api/todos", async (DataContext db, TodoItem todo) =>
{
    if (string.IsNullOrWhiteSpace(todo.Title)) return Results.BadRequest("Title required");
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.MapPut("/api/todos/{id:long}", async (DataContext db, long id, TodoItem updated) =>
{
    var existing = await db.Todos.FindAsync(id);
    if (existing is null) return Results.NotFound();

    existing.Title = updated.Title;
    existing.Description = updated.Description;
    existing.Selected = updated.Selected;
    existing.Completed = updated.Completed;

    existing.Category = updated.Category;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPatch("/api/todos/{id:long}/toggle-complete", async (DataContext db, long id) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Completed = !todo.Completed;
    await db.SaveChangesAsync();
    return Results.Ok(todo);
});

app.MapDelete("/api/todos/{id:long}", async (DataContext db, long id) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/api/todos/delete-selected", async (DataContext db, DeleteSelected.DeleteSelectedRequest req) =>
{
    if (req == null) return Results.BadRequest();
    if (req.Ids != null && req.Ids.Length > 0)
    {
        var items = db.Todos.Where(t => req.Ids.Contains(t.Id));
        db.Todos.RemoveRange(items);
        await db.SaveChangesAsync();
        return Results.Ok(new { deleted = req.Ids.Length });
    }
    if (!string.IsNullOrWhiteSpace(req.Category))
    {
        var items = db.Todos.Where(t => t.Category == req.Category);
        db.Todos.RemoveRange(items);
        await db.SaveChangesAsync();
        return Results.Ok(new { deletedByCategory = req.Category });
    }
    return Results.BadRequest("Provide ids or category");
});



// Run app
app.Run();