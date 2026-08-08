using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Talke.Api.Controllers;
using Talke.Application.DTOs.Auth;
using Talke.Domain.Entities;
using Talke.Domain.Enums;
using Talke.Domain.Repositories;
using Talke.Infrastructure.Security;

namespace Talke.Api.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IValidator<CreateUserRequestDto>> _validatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        // Arrange global (Organizando os mocks que serão usados em todos os testes)
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _validatorMock = new Mock<IValidator<CreateUserRequestDto>>();

        _controller = new AuthController(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Register_ComEmailJaExistente_DeveRetornar409Conflict()
    {
        // 1. Arrange (Preparação)
        var request = new CreateUserRequestDto("João", "Silva", "joao@teste.com", "senha123", UserRole.Student);

        // Simulando que a validação passou
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        // Simulando que o repositório encontrou o email (Vai retornar TRUE)
        _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(request.Email))
                           .ReturnsAsync(true);

        // 2. Act (Ação)
        var result = await _controller.Register(request);

        // 3. Assert (Verificação)
        var conflictResult = Assert.IsType<ConflictObjectResult>(result); // Verifica se o status é 409
        Assert.Equal(409, conflictResult.StatusCode);
    }

    [Fact]
    public async Task Register_ComDadosInvalidos_DeveRetornar400BadRequest()
    {
        // 1. Arrange
        var request = new CreateUserRequestDto("", "", "email-invalido", "123", UserRole.Student);
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Email", "Inválido") };

        // Simulando que o validador reprovou os dados
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult(validationFailures));

        // 2. Act
        var result = await _controller.Register(request);

        // 3. Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Verifica se o status é 400
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Register_ComDadosValidos_DeveRetornar201Created()
    {
        // 1. Arrange
        var request = new CreateUserRequestDto("Maria", "Souza", "maria@teste.com", "SenhaForte123", UserRole.Teacher);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult()); // Validação OK

        _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(request.Email))
                           .ReturnsAsync(false); // Email livre, não existe

        _passwordHasherMock.Setup(hash => hash.HashPassword(request.Password))
                           .Returns("hash_gerado_com_sucesso"); // Mockando o gerador de hash

        // 2. Act
        var result = await _controller.Register(request);

        // 3. Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result); // Verifica se o status é 201
        Assert.Equal(201, createdResult.StatusCode);

        // Verifica se o repositório foi chamado para salvar o usuário exatamente UMA vez
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
