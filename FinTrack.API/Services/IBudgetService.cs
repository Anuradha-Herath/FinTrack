using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface IBudgetService
{
    Task<IEnumerable<Budget>> GetAllAsync();
    Task<Budget?> GetByIdAsync(int id);
    Task<Budget> CreateAsync(Budget budget);
    Task<bool> UpdateAsync(int id, Budget budget);
    Task<bool> DeleteAsync(int id);
}