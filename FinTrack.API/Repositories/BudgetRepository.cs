using FinTrack.API.Data;
using FinTrack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.API.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly ApplicationDbContext _context;

    public BudgetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Budget>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Budgets
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Year)
            .ThenByDescending(b => b.Month)
            .ThenBy(b => b.Category)
            .ToListAsync();
    }

    public async Task<Budget?> GetByIdAsync(int id, int userId)
    {
        return await _context.Budgets
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<Budget?> GetByIdAsync(int id)
    {
        return await _context.Budgets.FindAsync(id);
    }

    public async Task<Budget> AddAsync(Budget budget)
    {
        budget.CreatedAt = DateTime.UtcNow;
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();
        return budget;
    }

    public async Task<bool> UpdateAsync(Budget budget)
    {
        budget.UpdatedAt = DateTime.UtcNow;
        _context.Budgets.Update(budget);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var budget = await GetByIdAsync(id, userId);
        if (budget == null) return false;

        _context.Budgets.Remove(budget);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<decimal> GetSpentAmountAsync(int userId, string category, int month, int year)
    {
        var spent = await _context.Transactions
            .Where(t => t.UserId == userId
                     && t.Category == category
                     && t.Type == "Expense"
                     && t.Date.Month == month
                     && t.Date.Year == year)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        return spent;
    }

    public async Task<bool> BudgetExistsAsync(int userId, string category, int month, int year, int? excludeBudgetId = null)
    {
        var query = _context.Budgets
            .Where(b => b.UserId == userId
                     && b.Category == category
                     && b.Month == month
                     && b.Year == year);

        if (excludeBudgetId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBudgetId.Value);
        }

        return await query.AnyAsync();
    }
}
