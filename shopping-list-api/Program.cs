using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Data;
using ShoppingListApi.DTOs;
using ShoppingListApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

app.UseCors("AllowAngularApp");

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

// User endpoints
app.MapGet("/api/users", async (ApplicationDbContext db) =>
{
    var users = await db.Users
        .Select(u => new UserDto(u.Id, u.Email, u.Name, u.IsAdmin, u.CreatedAt))
        .ToListAsync();
    return Results.Ok(users);
})
.WithName("GetUsers")
.WithTags("Users");

app.MapGet("/api/users/{id}", async (int id, ApplicationDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound();

    return Results.Ok(new UserDto(user.Id, user.Email, user.Name, user.IsAdmin, user.CreatedAt));
})
.WithName("GetUser")
.WithTags("Users");

app.MapPost("/api/users", async (CreateUserDto dto, ApplicationDbContext db) =>
{
    // Simple password hashing - in production use proper password hashing like BCrypt
    var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));

    var user = new User
    {
        Email = dto.Email,
        PasswordHash = passwordHash,
        Name = dto.Name,
        IsAdmin = dto.IsAdmin
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    var userDto = new UserDto(user.Id, user.Email, user.Name, user.IsAdmin, user.CreatedAt);
    return Results.Created($"/api/users/{user.Id}", userDto);
})
.WithName("CreateUser")
.WithTags("Users");

app.MapPut("/api/users/{id}", async (int id, UpdateUserDto dto, ApplicationDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound();

    user.Email = dto.Email;
    user.Name = dto.Name;
    user.IsAdmin = dto.IsAdmin;

    await db.SaveChangesAsync();

    return Results.Ok(new UserDto(user.Id, user.Email, user.Name, user.IsAdmin, user.CreatedAt));
})
.WithName("UpdateUser")
.WithTags("Users");

app.MapDelete("/api/users/{id}", async (int id, ApplicationDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteUser")
.WithTags("Users");

// ShoppingList endpoints
app.MapGet("/api/shopping-lists", async (ApplicationDbContext db) =>
{
    var lists = await db.ShoppingLists
        .Include(sl => sl.ShoppingListUsers)
        .Select(sl => new ShoppingListDto(
            sl.Id,
            sl.Name,
            sl.CreatedAt,
            sl.UpdatedAt,
            sl.ShoppingListUsers.Select(slu => slu.UserId).ToList()))
        .ToListAsync();
    return Results.Ok(lists);
})
.WithName("GetShoppingLists")
.WithTags("ShoppingLists");

app.MapGet("/api/shopping-lists/{id}", async (int id, ApplicationDbContext db) =>
{
    var list = await db.ShoppingLists
        .Include(sl => sl.ShoppingListUsers)
        .Where(sl => sl.Id == id)
        .Select(sl => new ShoppingListDto(
            sl.Id,
            sl.Name,
            sl.CreatedAt,
            sl.UpdatedAt,
            sl.ShoppingListUsers.Select(slu => slu.UserId).ToList()))
        .FirstOrDefaultAsync();

    if (list is null)
        return Results.NotFound();

    return Results.Ok(list);
})
.WithName("GetShoppingList")
.WithTags("ShoppingLists");

app.MapPost("/api/shopping-lists", async (CreateShoppingListDto dto, ApplicationDbContext db) =>
{
    var list = new ShoppingList
    {
        Name = dto.Name
    };

    db.ShoppingLists.Add(list);
    await db.SaveChangesAsync();

    // Add user associations
    foreach (var userId in dto.SharedWithUserIds)
    {
        db.ShoppingListUsers.Add(new ShoppingListUser
        {
            ShoppingListId = list.Id,
            UserId = userId
        });
    }
    await db.SaveChangesAsync();

    var listDto = new ShoppingListDto(
        list.Id,
        list.Name,
        list.CreatedAt,
        list.UpdatedAt,
        dto.SharedWithUserIds);

    return Results.Created($"/api/shopping-lists/{list.Id}", listDto);
})
.WithName("CreateShoppingList")
.WithTags("ShoppingLists");

app.MapPut("/api/shopping-lists/{id}", async (int id, UpdateShoppingListDto dto, ApplicationDbContext db) =>
{
    var list = await db.ShoppingLists
        .Include(sl => sl.ShoppingListUsers)
        .FirstOrDefaultAsync(sl => sl.Id == id);

    if (list is null)
        return Results.NotFound();

    list.Name = dto.Name;
    list.UpdatedAt = DateTime.UtcNow;

    // Update user associations
    db.ShoppingListUsers.RemoveRange(list.ShoppingListUsers);
    foreach (var userId in dto.SharedWithUserIds)
    {
        db.ShoppingListUsers.Add(new ShoppingListUser
        {
            ShoppingListId = list.Id,
            UserId = userId
        });
    }

    await db.SaveChangesAsync();

    var listDto = new ShoppingListDto(
        list.Id,
        list.Name,
        list.CreatedAt,
        list.UpdatedAt,
        dto.SharedWithUserIds);

    return Results.Ok(listDto);
})
.WithName("UpdateShoppingList")
.WithTags("ShoppingLists");

app.MapDelete("/api/shopping-lists/{id}", async (int id, ApplicationDbContext db) =>
{
    var list = await db.ShoppingLists.FindAsync(id);
    if (list is null)
        return Results.NotFound();

    db.ShoppingLists.Remove(list);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteShoppingList")
.WithTags("ShoppingLists");

// ShoppingListItem endpoints
app.MapGet("/api/shopping-lists/{listId}/items", async (int listId, ApplicationDbContext db) =>
{
    var items = await db.ShoppingListItems
        .Include(i => i.Category)
        .Include(i => i.AddedByUser)
        .Where(i => i.ShoppingListId == listId)
        .OrderBy(i => i.Category!.Order)
        .ThenBy(i => i.Order)
        .Select(i => new ShoppingListItemDto(
            i.Id,
            i.ShoppingListId,
            i.CategoryId,
            i.Category != null ? i.Category.Name : "",
            i.Name,
            i.Quantity,
            i.IsChecked,
            i.AddedBy,
            i.AddedByUser.Name,
            i.CreatedAt,
            i.Order))
        .ToListAsync();

    return Results.Ok(items);
})
.WithName("GetShoppingListItems")
.WithTags("ShoppingListItems");

app.MapGet("/api/shopping-list-items/{id}", async (int id, ApplicationDbContext db) =>
{
    var item = await db.ShoppingListItems
        .Include(i => i.Category)
        .Include(i => i.AddedByUser)
        .Where(i => i.Id == id)
        .Select(i => new ShoppingListItemDto(
            i.Id,
            i.ShoppingListId,
            i.CategoryId,
            i.Category != null ? i.Category.Name : "",
            i.Name,
            i.Quantity,
            i.IsChecked,
            i.AddedBy,
            i.AddedByUser.Name,
            i.CreatedAt,
            i.Order))
        .FirstOrDefaultAsync();

    if (item is null)
        return Results.NotFound();

    return Results.Ok(item);
})
.WithName("GetShoppingListItem")
.WithTags("ShoppingListItems");

app.MapPost("/api/shopping-list-items", async (CreateShoppingListItemDto dto, ApplicationDbContext db) =>
{
    var item = new ShoppingListItem
    {
        ShoppingListId = dto.ShoppingListId,
        CategoryId = dto.CategoryId,
        Name = dto.Name,
        Quantity = dto.Quantity,
        AddedBy = dto.AddedBy,
        Order = dto.Order
    };

    db.ShoppingListItems.Add(item);
    await db.SaveChangesAsync();

    // Reload with navigation properties
    var createdItem = await db.ShoppingListItems
        .Include(i => i.Category)
        .Include(i => i.AddedByUser)
        .FirstAsync(i => i.Id == item.Id);

    var itemDto = new ShoppingListItemDto(
        createdItem.Id,
        createdItem.ShoppingListId,
        createdItem.CategoryId,
        createdItem.Category?.Name ?? "",
        createdItem.Name,
        createdItem.Quantity,
        createdItem.IsChecked,
        createdItem.AddedBy,
        createdItem.AddedByUser.Name,
        createdItem.CreatedAt,
        createdItem.Order);

    return Results.Created($"/api/shopping-list-items/{item.Id}", itemDto);
})
.WithName("CreateShoppingListItem")
.WithTags("ShoppingListItems");

app.MapPut("/api/shopping-list-items/{id}", async (int id, UpdateShoppingListItemDto dto, ApplicationDbContext db) =>
{
    var item = await db.ShoppingListItems.FindAsync(id);
    if (item is null)
        return Results.NotFound();

    item.CategoryId = dto.CategoryId;
    item.Name = dto.Name;
    item.Quantity = dto.Quantity;
    item.IsChecked = dto.IsChecked;
    item.Order = dto.Order;

    await db.SaveChangesAsync();

    // Reload with navigation properties
    var updatedItem = await db.ShoppingListItems
        .Include(i => i.Category)
        .Include(i => i.AddedByUser)
        .FirstAsync(i => i.Id == item.Id);

    var itemDto = new ShoppingListItemDto(
        updatedItem.Id,
        updatedItem.ShoppingListId,
        updatedItem.CategoryId,
        updatedItem.Category?.Name ?? "",
        updatedItem.Name,
        updatedItem.Quantity,
        updatedItem.IsChecked,
        updatedItem.AddedBy,
        updatedItem.AddedByUser.Name,
        updatedItem.CreatedAt,
        updatedItem.Order);

    return Results.Ok(itemDto);
})
.WithName("UpdateShoppingListItem")
.WithTags("ShoppingListItems");

app.MapDelete("/api/shopping-list-items/{id}", async (int id, ApplicationDbContext db) =>
{
    var item = await db.ShoppingListItems.FindAsync(id);
    if (item is null)
        return Results.NotFound();

    db.ShoppingListItems.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteShoppingListItem")
.WithTags("ShoppingListItems");

app.Run();
