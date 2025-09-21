using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface IGoalService
{
    Task<IEnumerable<Goal>> GetAllAsync();
    Task<Goal?> GetByIdAsync(int id);
    Task<Goal> CreateAsync(Goal goal);
    Task<bool> UpdateAsync(int id, Goal goal);
    Task<bool> DeleteAsync(int id);
}