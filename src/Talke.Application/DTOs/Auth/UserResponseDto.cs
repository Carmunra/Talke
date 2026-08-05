public record UserResponseDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    UserRole Role // Enum que já temos no Domain
);