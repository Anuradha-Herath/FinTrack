using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class ReportService : IReportService
{
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public ReportService(IGenericRepository<Transaction> transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<object> GetSummaryAsync(int userId, int? month, int? year)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var query = transactions.Where(t => t.UserId == userId);

        if (month.HasValue && year.HasValue)
        {
            query = query.Where(t => t.Date.Month == month.Value && t.Date.Year == year.Value);
        }
        else if (year.HasValue)
        {
            query = query.Where(t => t.Date.Year == year.Value);
        }

        var filteredTransactions = query.ToList();

        var totalIncome = filteredTransactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
        var totalExpense = filteredTransactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        var netSavings = totalIncome - totalExpense;

        return new
        {
            totalIncome,
            totalExpense,
            netSavings
        };
    }

    public async Task<object> GetExpensesByCategoryAsync(int userId, int? month, int? year)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var query = transactions.Where(t => t.UserId == userId && t.Type == "Expense");

        if (month.HasValue && year.HasValue)
        {
            query = query.Where(t => t.Date.Month == month.Value && t.Date.Year == year.Value);
        }
        else if (year.HasValue)
        {
            query = query.Where(t => t.Date.Year == year.Value);
        }

        var filteredTransactions = query.ToList();

        var expensesByCategory = filteredTransactions
            .GroupBy(t => t.Category)
            .Select(g => new
            {
                category = g.Key,
                amount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.amount)
            .ToList();

        return expensesByCategory;
    }

    public async Task<object> GetIncomeVsExpenseTrendAsync(int userId, int year)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var query = transactions.Where(t => t.UserId == userId && t.Date.Year == year);

        var filteredTransactions = query.ToList();

        var trend = Enumerable.Range(1, 12)
            .Select(month => new
            {
                month = new DateTime(year, month, 1).ToString("MMM"),
                income = filteredTransactions
                    .Where(t => t.Type == "Income" && t.Date.Month == month)
                    .Sum(t => t.Amount),
                expense = filteredTransactions
                    .Where(t => t.Type == "Expense" && t.Date.Month == month)
                    .Sum(t => t.Amount)
            })
            .ToList();

        return trend;
    }

    public async Task<object> GetIncomeExpenseSummaryAsync(int userId)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        // Group by month for the last 6 months
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        var query = transactions.Where(t => t.UserId == userId && t.Date >= sixMonthsAgo);

        var filteredTransactions = query.ToList();

        var monthlyData = filteredTransactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new
            {
                month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                income = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                expenses = g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
            })
            .OrderBy(x => x.month)
            .ToList();

        return monthlyData;
    }

    public async Task<object> GetTransactionsByCategoryAsync(int userId)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var query = transactions.Where(t => t.UserId == userId && t.Type == "Expense");

        var filteredTransactions = query.ToList();

        var categoryData = filteredTransactions
            .GroupBy(t => t.Category)
            .Select(g => new
            {
                name = g.Key,
                value = g.Sum(t => t.Amount),
                percentage = 0 // Will be calculated on frontend if needed
            })
            .OrderByDescending(x => x.value)
            .ToList();

        return categoryData;
    }
}