namespace ShoppingListApi.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ShoppingListUser> ShoppingListUsers { get; set; } = new List<ShoppingListUser>();
    public ICollection<ShoppingListItem> AddedItems { get; set; } = new List<ShoppingListItem>();
}
