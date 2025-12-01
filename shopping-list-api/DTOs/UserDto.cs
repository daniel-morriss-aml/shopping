namespace ShoppingListApi.DTOs;

public record UserDto(int Id, string Email, string Name, bool IsAdmin, DateTime CreatedAt);

public record CreateUserDto(string Email, string Password, string Name, bool IsAdmin = false);

public record UpdateUserDto(string Email, string Name, bool IsAdmin);
