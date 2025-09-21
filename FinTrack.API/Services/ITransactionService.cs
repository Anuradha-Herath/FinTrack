using FinTrack.API.DTOs;

namespace FinTrack.API.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto> CreateAsync(TransactionDto dto);
    Task<bool> UpdateAsync(int id, TransactionDto dto);
    Task<bool> DeleteAsync(int id);
}