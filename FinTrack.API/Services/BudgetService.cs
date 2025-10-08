using FinTrack.API.DTOs;
using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<IEnumerable<BudgetResponseDto>> GetAllByUserIdAsync(int userId)
    {
        var budgets = await _budgetRepository.GetAllByUserIdAsync(userId);
        var budgetResponseDtos = new List<BudgetResponseDto>();

        foreach (var budget in budgets)
        {
            var spentAmount = await _budgetRepository.GetSpentAmountAsync(
                userId, budget.Category, budget.Month, budget.Year);
            
            budgetResponseDtos.Add(MapToBudgetResponseDto(budget, spentAmount));
        }

        return budgetResponseDtos;
    }

    public async Task<BudgetResponseDto?> GetByIdAsync(int id, int userId)
    {
        var budget = await _budgetRepository.GetByIdAsync(id, userId);
        if (budget == null) return null;

        var spentAmount = await _budgetRepository.GetSpentAmountAsync(
            userId, budget.Category, budget.Month, budget.Year);

        return MapToBudgetResponseDto(budget, spentAmount);
    }

    public async Task<BudgetResponseDto> CreateAsync(int userId, BudgetDto budgetDto)
    {
        // Check if budget already exists for this category, month, and year
        var exists = await _budgetRepository.BudgetExistsAsync(
            userId, budgetDto.Category, budgetDto.Month, budgetDto.Year);

        if (exists)
        {
            throw new InvalidOperationException(
                $"A budget for {budgetDto.Category} in {budgetDto.Month}/{budgetDto.Year} already exists.");
        }

        var budget = new Budget
        {
            UserId = userId,
            Category = budgetDto.Category,
            LimitAmount = budgetDto.LimitAmount,
            Month = budgetDto.Month,
            Year = budgetDto.Year
        };

        var createdBudget = await _budgetRepository.AddAsync(budget);
        
        var spentAmount = await _budgetRepository.GetSpentAmountAsync(
            userId, budget.Category, budget.Month, budget.Year);

        return MapToBudgetResponseDto(createdBudget, spentAmount);
    }

    public async Task<BudgetResponseDto?> UpdateAsync(int id, int userId, BudgetDto budgetDto)
    {
        var existingBudget = await _budgetRepository.GetByIdAsync(id, userId);
        if (existingBudget == null) return null;

        // Check if updating would create a duplicate
        var exists = await _budgetRepository.BudgetExistsAsync(
            userId, budgetDto.Category, budgetDto.Month, budgetDto.Year, id);

        if (exists)
        {
            throw new InvalidOperationException(
                $"A budget for {budgetDto.Category} in {budgetDto.Month}/{budgetDto.Year} already exists.");
        }

        existingBudget.Category = budgetDto.Category;
        existingBudget.LimitAmount = budgetDto.LimitAmount;
        existingBudget.Month = budgetDto.Month;
        existingBudget.Year = budgetDto.Year;

        await _budgetRepository.UpdateAsync(existingBudget);

        var spentAmount = await _budgetRepository.GetSpentAmountAsync(
            userId, existingBudget.Category, existingBudget.Month, existingBudget.Year);

        return MapToBudgetResponseDto(existingBudget, spentAmount);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        return await _budgetRepository.DeleteAsync(id, userId);
    }

    private BudgetResponseDto MapToBudgetResponseDto(Budget budget, decimal spentAmount)
    {
        var remainingAmount = budget.LimitAmount - spentAmount;
        var progressPercentage = budget.LimitAmount > 0 
            ? Math.Round((double)(spentAmount / budget.LimitAmount * 100), 2) 
            : 0;

        string status;
        if (progressPercentage >= 100)
            status = "over-budget";
        else if (progressPercentage >= 80)
            status = "warning";
        else
            status = "on-track";

        return new BudgetResponseDto
        {
            Id = budget.Id,
            UserId = budget.UserId,
            Category = budget.Category,
            LimitAmount = budget.LimitAmount,
            Month = budget.Month,
            Year = budget.Year,
            SpentAmount = spentAmount,
            RemainingAmount = remainingAmount,
            ProgressPercentage = progressPercentage,
            Status = status,
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt
        };
    }
}