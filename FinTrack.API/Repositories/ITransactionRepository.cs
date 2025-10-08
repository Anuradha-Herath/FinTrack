using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Transaction>> GetByUserIdWithFiltersAsync(int userId, string? type, string? category, DateTime? startDate, DateTime? endDate);
}