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
    
    public async Task CreateDefaultCategoriesAsync(string userName)
    {
    
        var baseCategories = new List<Category>
        {
            new Category { Name = "Rezsi", OwnerUserName = userName, ColorId = 1},
            new Category { Name = "Élelmiszer", OwnerUserName = userName, ColorId = 2 },
            new Category { Name = "Ruha", OwnerUserName = userName, ColorId = 3 },
            new Category { Name = "Egészség", OwnerUserName = userName, ColorId = 4 },
            new Category { Name = "Szépségápolás", OwnerUserName = userName, ColorId = 5 },
            new Category { Name = "Autó", OwnerUserName = userName, ColorId = 6 },
            new Category { Name = "Közlekedés", OwnerUserName = userName, ColorId = 7 },
            new Category { Name = "Lakbér", OwnerUserName = userName, ColorId = 8 },
            new Category { Name = "Ajándék", OwnerUserName = userName, ColorId = 9 },
            new Category { Name = "Elektronika", OwnerUserName = userName, ColorId = 10 },
            new Category { Name = "Háztartás", OwnerUserName = userName, ColorId = 11 },
            new Category { Name = "Háziállat", OwnerUserName = userName, ColorId = 12 }
        };

        _context.Categories.AddRange(baseCategories);
        await _context.SaveChangesAsync();
    }
}