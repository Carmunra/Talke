using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Talke.Application.DTOs.Auth;
using Talke.Domain.Entities;
using Talke.Domain.Repositories;
using Talke.Infrastructure.Security;

namespace Talke.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<CreateUserRequestDto> _validator;

    public AuthController(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IValidator<CreateUserRequestDto> validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto request)
    {
        // 1. Validação do payload (retorna 400 Bad Request)
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new { errors });
        }

        // 2. Verificar unicidade (retorna 409 Conflict)
        var emailExists = await _userRepository.ExistsByEmailAsync(request.Email);
        if (emailExists)
        {
            return Conflict("User with this email already exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            hashedPassword,
            request.Role);

        await _userRepository.AddAsync(user);

        return CreatedAtAction(nameof(Register), new { id = user.Id }, new UserResponseDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role
        ));
    }
}
