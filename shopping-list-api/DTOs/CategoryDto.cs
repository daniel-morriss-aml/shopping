namespace ShoppingListApi.DTOs;

public record CategoryDto(int Id, string Name, int Order, DateTime CreatedAt);

public record CreateCategoryDto(string Name, int Order);

public record UpdateCategoryDto(string Name, int Order);
