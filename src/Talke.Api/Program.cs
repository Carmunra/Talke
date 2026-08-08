using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Talke.Application.Validators;
using Talke.Domain.Repositories;
using Talke.Infrastructure.Data;
using Talke.Infrastructure.Repositories;
using Talke.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Contexto do banco de dados apontando para o PostgreSQL
builder.Services.AddDbContext<TalkeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

// Repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Serviço de hash de senha
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Validadores (FluentValidation)
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
