using FinTrack.API.DTOs;
using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<IEnumerable<GoalDto>> GetAllByUserIdAsync(int userId)
    {
        var goals = await _goalRepository.GetByUserIdAsync(userId);
        return goals.Select(MapToDto);
    }

    public async Task<GoalDto?> GetByIdAsync(int id, int userId)
    {
        var goal = await _goalRepository.GetByIdAsync(id);
        if (goal == null || goal.UserId != userId)
            return null;

        return MapToDto(goal);
    }

    public async Task<GoalDto> CreateAsync(GoalDto dto, int userId)
    {
        var goal = new Goal
        {
            UserId = userId,
            Title = dto.Title,
            TargetAmount = dto.TargetAmount,
            CurrentAmount = dto.CurrentAmount,
            Deadline = dto.Deadline,
            Description = dto.Description
        };

        await _goalRepository.AddAsync(goal);
        await _goalRepository.SaveChangesAsync();
        return MapToDto(goal);
    }

    public async Task<bool> UpdateAsync(int id, GoalDto dto, int userId)
    {
        var goal = await _goalRepository.GetByIdAsync(id);
        if (goal == null || goal.UserId != userId) return false;

        goal.Title = dto.Title;
        goal.TargetAmount = dto.TargetAmount;
        goal.CurrentAmount = dto.CurrentAmount;
        goal.Deadline = dto.Deadline;
        goal.Description = dto.Description;

        await _goalRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var goal = await _goalRepository.GetByIdAsync(id);
        if (goal == null || goal.UserId != userId) return false;

        _goalRepository.Remove(goal);
        await _goalRepository.SaveChangesAsync();
        return true;
    }

    private static GoalDto MapToDto(Goal goal)
    {
        return new GoalDto
        {
            Id = goal.Id,
            Title = goal.Title,
            TargetAmount = goal.TargetAmount,
            CurrentAmount = goal.CurrentAmount,
            Deadline = goal.Deadline,
            Description = goal.Description,
            ProgressPercentage = goal.TargetAmount > 0 ? (goal.CurrentAmount / goal.TargetAmount) * 100 : 0
        };
    }
}