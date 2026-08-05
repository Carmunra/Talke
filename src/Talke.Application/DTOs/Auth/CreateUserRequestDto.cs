public record CreateUserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role // Enum que já temos no Domain
);
