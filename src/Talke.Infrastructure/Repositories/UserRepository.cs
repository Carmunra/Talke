using Microsoft.EntityFrameworkCore;
using Talke.Domain.Entities;
using Talke.Domain.Repositories;
using Talke.Infrastructure.Data;

namespace Talke.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TalkeDbContext _context;

    public UserRepository(TalkeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await _context.Users.AnyAsync(u => u.Email == normalizedEmail);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
