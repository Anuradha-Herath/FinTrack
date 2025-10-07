using FinTrack.API.DTOs;
using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface IGoalService
{
    Task<IEnumerable<GoalDto>> GetAllByUserIdAsync(int userId);
    Task<GoalDto?> GetByIdAsync(int id, int userId);
    Task<GoalDto> CreateAsync(GoalDto dto, int userId);
    Task<bool> UpdateAsync(int id, GoalDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}