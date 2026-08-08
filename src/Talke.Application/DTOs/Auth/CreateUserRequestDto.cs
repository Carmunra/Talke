using Talke.Domain.Enums;

namespace Talke.Application.DTOs.Auth;

public record CreateUserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role
);
