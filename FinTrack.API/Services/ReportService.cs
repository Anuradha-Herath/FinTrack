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

    public async Task<object> GetSummaryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));
        return new { TotalIncome = totalIncome, TotalExpense = totalExpense, Balance = totalIncome - totalExpense };
    }

    public async Task<object> GetTransactionsByCategoryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var grouped = transactions.GroupBy(t => t.CategoryId)
            .Select(g => new { CategoryId = g.Key, Total = g.Sum(t => t.Amount) });
        return grouped;
    }
}