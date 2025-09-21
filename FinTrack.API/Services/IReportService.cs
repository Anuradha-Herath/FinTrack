namespace FinTrack.API.Services;

public interface IReportService
{
    Task<object> GetSummaryAsync();
    Task<object> GetTransactionsByCategoryAsync();
}