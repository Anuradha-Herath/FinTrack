using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class ReportService : IReportService
{
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly IGenericRepository<Category> _categoryRepository;

    public ReportService(
        IGenericRepository<Transaction> transactionRepository,
        IGenericRepository<Category> categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<object> GetSummaryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var totalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));
        var netBalance = totalIncome - totalExpenses;
        
        return new
        {
            totalIncome,
            totalExpenses,
            netBalance
        };
    }

    public async Task<object> GetIncomeExpenseSummaryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        
        // Group by month for the last 6 months
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var monthlyData = transactions
            .Where(t => t.Date >= sixMonthsAgo)
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new
            {
                month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                income = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                expenses = Math.Abs(g.Where(t => t.Amount < 0).Sum(t => t.Amount))
            })
            .OrderBy(x => x.month)
            .ToList();

        return monthlyData;
    }

    public async Task<object> GetTransactionsByCategoryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();
        
        // Only get expenses (negative amounts)
        var expenseTransactions = transactions.Where(t => t.Amount < 0).ToList();
        var totalExpenses = expenseTransactions.Sum(t => Math.Abs(t.Amount));
        
        var categoryData = expenseTransactions
            .GroupBy(t => t.CategoryId)
            .Select(g =>
            {
                var category = categories.FirstOrDefault(c => c.Id == g.Key);
                var value = Math.Abs(g.Sum(t => t.Amount));
                var percentage = totalExpenses > 0 ? Math.Round((value / totalExpenses) * 100, 1) : 0;
                
                return new
                {
                    name = category?.Name ?? "Uncategorized",
                    value,
                    percentage
                };
            })
            .OrderByDescending(x => x.value)
            .ToList();

        return categoryData;
    }
}