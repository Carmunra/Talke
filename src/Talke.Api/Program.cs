using Microsoft.EntityFrameworkCore;
using Talke.Infrastructure.Data;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adicionar o contexto do banco de dados apontado para o PostgreSQL
builder.Services.AddDbContext<TalkeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// builder.Services.AddDbContext<TalkeDbContext>(...);

// 1. Registrando o Repositório
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 2. Registrando o Serviço de Hash de Senha
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// 3. Registrando os Validadores (FluentValidation)
// O pacote FluentValidation.AspNetCore faz isso automaticamente se você usar:
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

