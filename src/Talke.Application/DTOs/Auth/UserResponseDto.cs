using Talke.Domain.Enums;

namespace Talke.Application.DTOs.Auth;

public record UserResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    UserRole Role
);
