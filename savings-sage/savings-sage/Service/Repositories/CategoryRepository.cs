using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;

namespace savings_sage.Service.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly UsersContext _context;

    public CategoryRepository(UsersContext context)
    {
        _context = context;
    }
    
    public async Task CreateDefaultCategoriesAsync(string userId)
    {
    
        var baseCategories = new List<Category>
        {
            new() { Name = "Utilities", OwnerId = userId, ColorId = 1, IconURL = "/icons/lightbulb.svg"},
            new() { Name = "Food", OwnerId = userId, ColorId = 2, IconURL = "/icons/utensils.svg"},
            new() { Name = "Clothing", OwnerId = userId, ColorId = 3, IconURL = "/icons/hanger.svg"},
            new() { Name = "Health", OwnerId = userId, ColorId = 4, IconURL = "/icons/heart-medical.svg" },
            new() { Name = "Beauty", OwnerId = userId, ColorId = 5, IconURL = "/icons/sanitizer.svg"},
            new() { Name = "Car", OwnerId = userId, ColorId = 6, IconURL = "/icons/car-sideview.svg"},
            new() { Name = "Transportation", OwnerId = userId, ColorId = 7, IconURL = "/icons/subway.svg"},
            new() { Name = "Rent", OwnerId = userId, ColorId = 8, IconURL = "/icons/key.svg"},
            new() { Name = "Gifts", OwnerId = userId, ColorId = 9, IconURL = "/icons/gift.svg"},
            new() { Name = "Electronics", OwnerId = userId, ColorId = 10, IconURL = "/icons/desktop-alt.svg"},
            new() { Name = "Household", OwnerId = userId, ColorId = 11, IconURL = "/icons/house.svg"},
            new() { Name = "Pet", OwnerId = userId, ColorId = 12, IconURL = "/icons/paw.svg"}
        };

        _context.Categories.AddRange(baseCategories);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Category>> GetCategoriesByUser(string userId)
    {
        var categoriesByUser = await _context.Categories.Where(x => x.OwnerId == userId).ToListAsync();
        return categoriesByUser;
    }

    public async Task<Category?> GetByIdAsync(int catId)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == catId);
    }

    public async Task<Category> NewCategoryAsync(string userId, string categoryName, int colorId)
    {
        var newCategory = new Category { Name = categoryName, OwnerId = userId, ColorId = colorId };
        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return newCategory;
    }

    public async Task DeleteCategory(int id)
    {
        var categoryToRemove = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        _context.Categories.Remove(categoryToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategory(int id, string categoryName, int colorId)
    {
        var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        
        categoryToUpdate.Name = categoryName;
        categoryToUpdate.ColorId = colorId;
        
        _context.Categories.Update(categoryToUpdate);
        await _context.SaveChangesAsync();
    }
}