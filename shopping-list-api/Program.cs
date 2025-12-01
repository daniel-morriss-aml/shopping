using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Data;
using ShoppingListApi.DTOs;
using ShoppingListApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=shoppinglist.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Category endpoints
app.MapGet("/api/categories", async (ApplicationDbContext db) =>
{
    var categories = await db.Categories
        .OrderBy(c => c.Order)
        .Select(c => new CategoryDto(c.Id, c.Name, c.Order, c.CreatedAt))
        .ToListAsync();
    return Results.Ok(categories);
})
.WithName("GetCategories")
.WithTags("Categories");

app.MapGet("/api/categories/{id}", async (int id, ApplicationDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    if (category is null)
        return Results.NotFound();

    return Results.Ok(new CategoryDto(category.Id, category.Name, category.Order, category.CreatedAt));
})
.WithName("GetCategory")
.WithTags("Categories");

app.MapPost("/api/categories", async (CreateCategoryDto dto, ApplicationDbContext db) =>
{
    var category = new Category
    {
        Name = dto.Name,
        Order = dto.Order
    };

    db.Categories.Add(category);
    await db.SaveChangesAsync();

    var categoryDto = new CategoryDto(category.Id, category.Name, category.Order, category.CreatedAt);
    return Results.Created($"/api/categories/{category.Id}", categoryDto);
})
.WithName("CreateCategory")
.WithTags("Categories");

app.MapPut("/api/categories/{id}", async (int id, UpdateCategoryDto dto, ApplicationDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    if (category is null)
        return Results.NotFound();

    category.Name = dto.Name;
    category.Order = dto.Order;

    await db.SaveChangesAsync();

    return Results.Ok(new CategoryDto(category.Id, category.Name, category.Order, category.CreatedAt));
})
.WithName("UpdateCategory")
.WithTags("Categories");

app.MapDelete("/api/categories/{id}", async (int id, ApplicationDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);
    if (category is null)
        return Results.NotFound();

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteCategory")
.WithTags("Categories");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
