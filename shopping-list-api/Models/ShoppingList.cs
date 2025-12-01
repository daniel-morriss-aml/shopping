namespace ShoppingListApi.Models;

public class ShoppingList
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
    public ICollection<ShoppingListUser> ShoppingListUsers { get; set; } = new List<ShoppingListUser>();
}
