using FinTrack.API.Models;

namespace FinTrack.API.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<bool> UpdateAsync(int id, Category category);
    Task<bool> DeleteAsync(int id);
}