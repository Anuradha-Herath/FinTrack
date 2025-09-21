using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class GoalService : IGoalService
{
    private readonly IGenericRepository<Goal> _goalRepository;

    public GoalService(IGenericRepository<Goal> goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        return await _goalRepository.GetAllAsync();
    }

    public async Task<Goal?> GetByIdAsync(int id)
    {
        return await _goalRepository.GetByIdAsync(id);
    }

    public async Task<Goal> CreateAsync(Goal goal)
    {
        await _goalRepository.AddAsync(goal);
        await _goalRepository.SaveChangesAsync();
        return goal;
    }

    public async Task<bool> UpdateAsync(int id, Goal goal)
    {
        var existing = await _goalRepository.GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = goal.Name;
        existing.Description = goal.Description;
        existing.TargetAmount = goal.TargetAmount;
        existing.CurrentAmount = goal.CurrentAmount;
        existing.TargetDate = goal.TargetDate;
        await _goalRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var goal = await _goalRepository.GetByIdAsync(id);
        if (goal == null) return false;
        _goalRepository.Remove(goal);
        await _goalRepository.SaveChangesAsync();
        return true;
    }
}