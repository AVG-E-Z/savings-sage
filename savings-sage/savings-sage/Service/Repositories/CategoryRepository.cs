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
            new Category { Name = "Rezsi", OwnerId = userId, ColorId = 1},
            new Category { Name = "Élelmiszer", OwnerId = userId, ColorId = 2 },
            new Category { Name = "Ruha", OwnerId = userId, ColorId = 3 },
            new Category { Name = "Egészség", OwnerId = userId, ColorId = 4 },
            new Category { Name = "Szépségápolás", OwnerId = userId, ColorId = 5 },
            new Category { Name = "Autó", OwnerId = userId, ColorId = 6 },
            new Category { Name = "Közlekedés", OwnerId = userId, ColorId = 7 },
            new Category { Name = "Lakbér", OwnerId = userId, ColorId = 8 },
            new Category { Name = "Ajándék", OwnerId = userId, ColorId = 9 },
            new Category { Name = "Elektronika", OwnerId = userId, ColorId = 10 },
            new Category { Name = "Háztartás", OwnerId = userId, ColorId = 11 },
            new Category { Name = "Háziállat", OwnerId = userId, ColorId = 12 }
        };

        context.Categories.AddRange(baseCategories);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesByUser(string userId)
    {
        var categoriesByUser = await context.Categories.Where(x => x.OwnerId == userId).ToListAsync();
        return categoriesByUser;
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