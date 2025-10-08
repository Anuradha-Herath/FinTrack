using FinTrack.API.DTOs;
using FinTrack.API.Models;
using FinTrack.API.Repositories;

namespace FinTrack.API.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllByUserIdAsync(int userId)
    {
        var transactions = await _transactionRepository.GetByUserIdAsync(userId);
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<TransactionDto>> GetByUserIdWithFiltersAsync(
        int userId, 
        string? type, 
        string? category, 
        DateTime? startDate, 
        DateTime? endDate)
    {
        var transactions = await _transactionRepository.GetByUserIdWithFiltersAsync(
            userId, type, category, startDate, endDate);
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<TransactionDto>> GetRecentByUserIdAsync(int userId, int limit = 10)
    {
        var transactions = await _transactionRepository.GetByUserIdAsync(userId);
        return transactions
            .OrderByDescending(t => t.Date)
            .Take(limit)
            .Select(MapToDto);
    }

    public async Task<TransactionDto?> GetByIdAsync(int id, int userId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null || transaction.UserId != userId) 
            return null;
        
        return MapToDto(transaction);
    }

    public async Task<TransactionDto> CreateAsync(TransactionDto dto, int userId)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            Type = dto.Type,
            Category = dto.Category,
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            AccountId = dto.AccountId
        };

        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        // Update account balance if account is specified
        if (dto.AccountId.HasValue)
        {
            await UpdateAccountBalance(dto.AccountId.Value, dto.Amount, dto.Type);
        }

        // Update user totals
        await UpdateUserTotals(userId, dto.Amount, dto.Type);

        dto.Id = transaction.Id;
        return dto;
    }

    public async Task<bool> UpdateAsync(int id, TransactionDto dto, int userId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null || transaction.UserId != userId) 
            return false;

        // Revert old account balance
        if (transaction.AccountId.HasValue)
        {
            await UpdateAccountBalance(transaction.AccountId.Value, transaction.Amount, 
                transaction.Type == "Income" ? "Expense" : "Income");
        }

        // Revert old user totals
        await UpdateUserTotals(userId, transaction.Amount, transaction.Type == "Income" ? "Expense" : "Income");

        transaction.Type = dto.Type;
        transaction.Category = dto.Category;
        transaction.Amount = dto.Amount;
        transaction.Date = dto.Date;
        transaction.Description = dto.Description;
        transaction.AccountId = dto.AccountId;

        await _transactionRepository.SaveChangesAsync();

        // Apply new account balance
        if (dto.AccountId.HasValue)
        {
            await UpdateAccountBalance(dto.AccountId.Value, dto.Amount, dto.Type);
        }

        // Apply new user totals
        await UpdateUserTotals(userId, dto.Amount, dto.Type);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null || transaction.UserId != userId) 
            return false;

        // Revert account balance
        if (transaction.AccountId.HasValue)
        {
            await UpdateAccountBalance(transaction.AccountId.Value, transaction.Amount, 
                transaction.Type == "Income" ? "Expense" : "Income");
        }

        // Revert user totals
        await UpdateUserTotals(userId, transaction.Amount, transaction.Type == "Income" ? "Expense" : "Income");

        _transactionRepository.Remove(transaction);
        await _transactionRepository.SaveChangesAsync();
        return true;
    }

    public async Task<Dictionary<string, decimal>> GetSummaryAsync(int userId)
    {
        // Get all transactions for the user
        var transactions = await _transactionRepository.GetAllAsync();
        var userTransactions = transactions.Where(t => t.UserId == userId).ToList();

        // Calculate totals from actual transactions
        var totalIncome = userTransactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
        var totalExpense = userTransactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        var balance = totalIncome - totalExpense;

        // Update user totals in database for consistency
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null && (user.TotalIncome != totalIncome || user.TotalExpense != totalExpense))
        {
            user.TotalIncome = totalIncome;
            user.TotalExpense = totalExpense;
            await _userRepository.SaveChangesAsync();
        }

        return new Dictionary<string, decimal>
        {
            { "totalIncome", totalIncome },
            { "totalExpense", totalExpense },
            { "balance", balance }
        };
    }

    private async Task UpdateAccountBalance(int accountId, decimal amount, string type)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account != null)
        {
            if (type == "Income")
            {
                account.Balance += amount;
            }
            else if (type == "Expense")
            {
                account.Balance -= amount;
            }
            await _accountRepository.SaveChangesAsync();
        }
    }

    private async Task UpdateUserTotals(int userId, decimal amount, string type)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            if (type == "Income")
            {
                user.TotalIncome += amount;
            }
            else if (type == "Expense")
            {
                user.TotalExpense += amount;
            }
            await _userRepository.SaveChangesAsync();
        }
    }

    private TransactionDto MapToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            Type = transaction.Type,
            Category = transaction.Category,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Description = transaction.Description,
            AccountId = transaction.AccountId,
            AccountName = transaction.Account?.Name
        };
    }
}