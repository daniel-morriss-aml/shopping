namespace ShoppingListApi.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
}
