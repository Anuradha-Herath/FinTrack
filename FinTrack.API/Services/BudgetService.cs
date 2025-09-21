using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class BudgetService : IBudgetService
{
    private readonly IGenericRepository<Budget> _budgetRepository;

    public BudgetService(IGenericRepository<Budget> budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<IEnumerable<Budget>> GetAllAsync()
    {
        return await _budgetRepository.GetAllAsync();
    }

    public async Task<Budget?> GetByIdAsync(int id)
    {
        return await _budgetRepository.GetByIdAsync(id);
    }

    public async Task<Budget> CreateAsync(Budget budget)
    {
        await _budgetRepository.AddAsync(budget);
        await _budgetRepository.SaveChangesAsync();
        return budget;
    }

    public async Task<bool> UpdateAsync(int id, Budget budget)
    {
        var existing = await _budgetRepository.GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = budget.Name;
        existing.Amount = budget.Amount;
        existing.StartDate = budget.StartDate;
        existing.EndDate = budget.EndDate;
        existing.CategoryId = budget.CategoryId;
        await _budgetRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var budget = await _budgetRepository.GetByIdAsync(id);
        if (budget == null) return false;
        _budgetRepository.Remove(budget);
        await _budgetRepository.SaveChangesAsync();
        return true;
    }
}