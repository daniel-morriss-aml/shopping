namespace ShoppingListApi.Models;

public class ShoppingListItem
{
    public int Id { get; set; }
    public int ShoppingListId { get; set; }
    public int? CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Quantity { get; set; }
    public bool IsChecked { get; set; }
    public int AddedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Order { get; set; }

    // Navigation properties
    public ShoppingList ShoppingList { get; set; } = null!;
    public Category? Category { get; set; }
    public User AddedByUser { get; set; } = null!;
}
