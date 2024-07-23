namespace savings_sage.Service.Repositories;

public interface ICategoryService
{
    Task CreateDefaultCategoriesAsync(string userId);
}