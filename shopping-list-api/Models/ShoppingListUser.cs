namespace ShoppingListApi.Models;

public class ShoppingListUser
{
    public int ShoppingListId { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public ShoppingList ShoppingList { get; set; } = null!;
    public User User { get; set; } = null!;
}
