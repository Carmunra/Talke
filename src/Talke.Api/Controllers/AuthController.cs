using Microsoft.AspNetCore.Mvc;
using Talke.Domain.Entities;

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
        //1. Validação do Payload (Retorna 400 Bad Request)
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        //2. Verificar Unicidade (Retorna 409 Conflict)
        var emailExists = await _userRepository.ExistsByEmailAsync(request.Email);
        if (emailExists)
        {
            return Conflict("User with this email already exists.");
        }

        // Hash the password
        var hashedPassword = _passwordHasher.Hash(request.Password);

        // Create the user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            Role = request.Role
        };

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
