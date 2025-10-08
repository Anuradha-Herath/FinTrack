using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public interface IBudgetRepository
{
    Task<IEnumerable<Budget>> GetAllByUserIdAsync(int userId);
    Task<Budget?> GetByIdAsync(int id, int userId);
    Task<Budget?> GetByIdAsync(int id);
    Task<Budget> AddAsync(Budget budget);
    Task<bool> UpdateAsync(Budget budget);
    Task<bool> DeleteAsync(int id, int userId);
    Task<decimal> GetSpentAmountAsync(int userId, string category, int month, int year);
    Task<bool> BudgetExistsAsync(int userId, string category, int month, int year, int? excludeBudgetId = null);
}
