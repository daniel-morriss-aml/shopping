namespace ShoppingListApi.DTOs;

public record ShoppingListItemDto(
    int Id,
    int ShoppingListId,
    int? CategoryId,
    string CategoryName,
    string Name,
    string? Quantity,
    bool IsChecked,
    int AddedBy,
    string AddedByName,
    DateTime CreatedAt,
    int Order);

public record CreateShoppingListItemDto(
    int ShoppingListId,
    int? CategoryId,
    string Name,
    string? Quantity,
    int AddedBy,
    int Order);

public record UpdateShoppingListItemDto(
    int? CategoryId,
    string Name,
    string? Quantity,
    bool IsChecked,
    int Order);
