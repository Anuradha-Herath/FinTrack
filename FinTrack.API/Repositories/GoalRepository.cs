using FinTrack.API.Data;
using FinTrack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.API.Repositories;

public class GoalRepository : GenericRepository<Goal>, IGoalRepository
{
    public GoalRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Goal>> GetByUserIdAsync(int userId)
    {
        return await _context.Goals
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.Deadline)
            .ToListAsync();
    }
}