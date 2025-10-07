using FinTrack.API.DTOs;

namespace FinTrack.API.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllByUserIdAsync(int userId);
    Task<IEnumerable<TransactionDto>> GetByUserIdWithFiltersAsync(int userId, string? type, string? category, DateTime? startDate, DateTime? endDate);
    Task<TransactionDto?> GetByIdAsync(int id, int userId);
    Task<TransactionDto> CreateAsync(TransactionDto dto, int userId);
    Task<bool> UpdateAsync(int id, TransactionDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<Dictionary<string, decimal>> GetSummaryAsync(int userId);
}