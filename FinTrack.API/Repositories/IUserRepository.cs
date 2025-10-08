using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
}