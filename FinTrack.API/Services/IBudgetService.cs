using FinTrack.API.DTOs;
using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface IBudgetService
{
    Task<IEnumerable<BudgetResponseDto>> GetAllByUserIdAsync(int userId);
    Task<BudgetResponseDto?> GetByIdAsync(int id, int userId);
    Task<BudgetResponseDto> CreateAsync(int userId, BudgetDto budgetDto);
    Task<BudgetResponseDto?> UpdateAsync(int id, int userId, BudgetDto budgetDto);
    Task<bool> DeleteAsync(int id, int userId);
}