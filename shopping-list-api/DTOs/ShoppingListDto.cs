namespace ShoppingListApi.DTOs;

public record ShoppingListDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<int> SharedWithUserIds);

public record CreateShoppingListDto(string Name, List<int> SharedWithUserIds);

public record UpdateShoppingListDto(string Name, List<int> SharedWithUserIds);
