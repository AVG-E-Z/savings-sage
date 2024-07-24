using savings_sage.Model;

namespace savings_sage.Service.Repositories;

public interface ICategoryRepository
{
    Task CreateDefaultCategoriesAsync(string userId);
    public Task<IEnumerable<Category>> GetCategoriesByUser(string userId);
    public Task<Category?> GetByIdAsync(int catId);
    public Task<Category> NewCategoryAsync(string userId, string categoryName, int colorId);
    public Task DeleteCategory(int id);
    public Task UpdateCategory(int id, string categoryName, int colorId);
}