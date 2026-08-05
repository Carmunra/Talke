using Microsoft.EntityFrameworkCore;
using Talke.Domain.Entities;
using Talke.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly TalkeDbContext _context;

    public UserRepository(TalkeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        // Vai no banco e verifica se já existe um usuário com o email fornecido
        return await _context.Set<User>().AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        // Adiciona o usuário ao contexto e salva as alterações
        await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();
    }
}