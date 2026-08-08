using Talke.Domain.Entities;

namespace Talke.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user);
}
