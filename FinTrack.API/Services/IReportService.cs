namespace FinTrack.API.Services;

public interface IReportService
{
    Task<object> GetSummaryAsync(int userId, int? month, int? year);
    Task<object> GetExpensesByCategoryAsync(int userId, int? month, int? year);
    Task<object> GetIncomeVsExpenseTrendAsync(int userId, int year);
    Task<object> GetIncomeExpenseSummaryAsync(int userId);
    Task<object> GetTransactionsByCategoryAsync(int userId);
}