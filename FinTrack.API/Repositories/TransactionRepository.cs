using FinTrack.API.Data;
using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }
}