using FinTrack.API.Data;
using FinTrack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.API.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdWithFiltersAsync(
        int userId, 
        string? type, 
        string? category, 
        DateTime? startDate, 
        DateTime? endDate)
    {
        var query = _context.Transactions
            .Include(t => t.Account)
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(t => t.Type == type);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(t => t.Category == category);
        }

        if (startDate.HasValue)
        {
            query = query.Where(t => t.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.Date <= endDate.Value);
        }

        return await query.OrderByDescending(t => t.Date).ToListAsync();
    }
}