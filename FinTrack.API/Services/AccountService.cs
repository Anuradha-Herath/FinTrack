using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _accountRepository.GetAllAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }

    public async Task<Account> CreateAsync(Account account)
    {
        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();
        return account;
    }

    public async Task<bool> UpdateAsync(int id, Account account)
    {
        var existing = await _accountRepository.GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = account.Name;
        existing.Type = account.Type;
        existing.Balance = account.Balance;
        await _accountRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null) return false;
        _accountRepository.Remove(account);
        await _accountRepository.SaveChangesAsync();
        return true;
    }
}