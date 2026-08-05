using System.Threading.Tasks;
using Talke.Domain.Entities;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user); // Assumindo que Student e Teacher herdam de uma classe base User
}