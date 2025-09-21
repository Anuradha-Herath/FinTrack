using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(int id);
    Task<Account> CreateAsync(Account account);
    Task<bool> UpdateAsync(int id, Account account);
    Task<bool> DeleteAsync(int id);
}