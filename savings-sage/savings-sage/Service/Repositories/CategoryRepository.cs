using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;

namespace savings_sage.Service.Repositories;


public class CategoryRepository(UsersContext context) : ICategoryRepository
{
    public async Task CreateDefaultCategoriesAsync(string userId)
    {
    
        var baseCategories = new List<Category>
        {
            new() { Name = "Utilities", OwnerId = userId, ColorId = 1, IconURL = "/icons/lightbulb.svg"},
            new() { Name = "Food", OwnerId = userId, ColorId = 2, IconURL = "/icons/utensils.svg"},
            new() { Name = "Clothing", OwnerId = userId, ColorId = 3, IconURL = "/icons/hanger.svg"},
            new() { Name = "Health", OwnerId = userId, ColorId = 4, IconURL = "/icons/heart-medical.svg" },
            new() { Name = "Beauty", OwnerId = userId, ColorId = 5, IconURL = "/icons/sanitizer-alt.svg"},
            new() { Name = "Car", OwnerId = userId, ColorId = 6, IconURL = "/icons/car-sideview.svg"},
            new() { Name = "Commute", OwnerId = userId, ColorId = 7, IconURL = "/icons/subway.svg"},
            new() { Name = "Rent", OwnerId = userId, ColorId = 8, IconURL = "/icons/key.svg"},
            new() { Name = "Gifts", OwnerId = userId, ColorId = 9, IconURL = "/icons/gift.svg"},
            new() { Name = "Electronics", OwnerId = userId, ColorId = 10, IconURL = "/icons/desktop-alt.svg"},
            new() { Name = "Household", OwnerId = userId, ColorId = 11, IconURL = "/icons/house.svg"},
            new() { Name = "Pet", OwnerId = userId, ColorId = 12, IconURL = "/icons/paw.svg"}
        };



        context.Categories.AddRange(baseCategories);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesByUser(string userId)
    {
        var categoriesByUser = await context.Categories.Where(x => x.OwnerId == userId).ToListAsync();
        
         var categoriesDtos = categoriesByUser.Select(cat => new CategoryDto()
        { ColorId = cat.ColorId, IconURL = cat.IconURL, Id = cat.Id, Name = cat.Name,
            OwnerId = cat.OwnerId
        });
        return categoriesDtos;
    }

    public async Task<Category?> GetByIdAsync(int catId)
    {

        return await context.Categories.FirstOrDefaultAsync(x => x.Id == catId);
    }

    public async Task<Category> NewCategoryAsync(string userId, string categoryName, int colorId)
    {
        var newCategory = new Category { Name = categoryName, OwnerId = userId, ColorId = colorId };
        await context.Categories.AddAsync(newCategory);
        await context.SaveChangesAsync();
        return newCategory;
    }

    public async Task DeleteCategory(int id)
    {
        var categoryToRemove = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        context.Categories.Remove(categoryToRemove);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCategory(int id, string categoryName, int colorId)
    {
        var categoryToUpdate = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        
        categoryToUpdate.Name = categoryName;
        categoryToUpdate.ColorId = colorId;
       
        context.Categories.Update(categoryToUpdate);
        await context.SaveChangesAsync();


    }
}