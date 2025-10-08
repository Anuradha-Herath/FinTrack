using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public interface IGoalRepository : IGenericRepository<Goal>
{
    Task<IEnumerable<Goal>> GetByUserIdAsync(int userId);
}