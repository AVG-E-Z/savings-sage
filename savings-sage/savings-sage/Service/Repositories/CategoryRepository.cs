using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;

namespace savings_sage.Service.Repositories;


public class CategoryRepository(SavingsSageContext context) : ICategoryRepository
{
    public async Task CreateDefaultCategoriesAsync(string userId)
    {
        var colors = await context.Colors.Where(x => x.Id < 13).ToListAsync();
    
        var baseCategories = new List<Category>
        {
            new() { Name = "Correction", OwnerId = userId, ColorId = 8, IconURL = "/icons/wrench.svg", Color = colors.FirstOrDefault(x => x.Id == 8)},
            new() { Name = "Utilities", OwnerId = userId, ColorId = 1, IconURL = "/icons/lightbulb.svg", Color = colors.FirstOrDefault(x => x.Id == 1)},
            new() { Name = "Food", OwnerId = userId, ColorId = 2, IconURL = "/icons/utensils.svg", Color = colors.FirstOrDefault(x => x.Id == 2)},
            new() { Name = "Clothing", OwnerId = userId, ColorId = 3, IconURL = "/icons/hanger.svg", Color = colors.FirstOrDefault(x => x.Id == 3)},
            new() { Name = "Health", OwnerId = userId, ColorId = 4, IconURL = "/icons/heart-medical.svg", Color = colors.FirstOrDefault(x => x.Id == 4) },
            new() { Name = "Beauty", OwnerId = userId, ColorId = 5, IconURL = "/icons/sanitizer-alt.svg", Color = colors.FirstOrDefault(x => x.Id == 5)},
            new() { Name = "Car", OwnerId = userId, ColorId = 6, IconURL = "/icons/car-sideview.svg", Color = colors.FirstOrDefault(x => x.Id == 6)},
            new() { Name = "Commute", OwnerId = userId, ColorId = 7, IconURL = "/icons/subway.svg", Color = colors.FirstOrDefault(x => x.Id == 7)},
            new() { Name = "Rent", OwnerId = userId, ColorId = 8, IconURL = "/icons/key.svg", Color = colors.FirstOrDefault(x => x.Id == 8)},
            new() { Name = "Gifts", OwnerId = userId, ColorId = 9, IconURL = "/icons/gift.svg", Color = colors.FirstOrDefault(x => x.Id == 9)},
            new() { Name = "Electronics", OwnerId = userId, ColorId = 10, IconURL = "/icons/desktop-alt.svg", Color = colors.FirstOrDefault(x => x.Id == 10)},
            new() { Name = "Household", OwnerId = userId, ColorId = 11, IconURL = "/icons/house.svg", Color = colors.FirstOrDefault(x => x.Id == 11)},
            new() { Name = "Pet", OwnerId = userId, ColorId = 12, IconURL = "/icons/paw.svg", Color = colors.FirstOrDefault(x => x.Id == 12)}
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

    public async Task<Category> GetCategoryByUserById(string userId, int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.OwnerId == userId && x.Id == id);
        var color = await context.Colors.FirstOrDefaultAsync(x => x.Id == category.ColorId);
        category.Color = color;
        await context.SaveChangesAsync();
        return category;
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