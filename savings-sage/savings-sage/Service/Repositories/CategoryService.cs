using savings_sage.Context;
using savings_sage.Model;

namespace savings_sage.Service.Repositories;

public class CategoryService : ICategoryService
{
    private readonly UsersContext _context;

    public CategoryService(UsersContext context)
    {
        _context = context;
    }
    
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

        _context.Categories.AddRange(baseCategories);
        await _context.SaveChangesAsync();
    }
}