using FinTrack.API.Data;
using FinTrack.API.Models;

namespace FinTrack.API.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }
}