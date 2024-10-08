using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;
using savings_sage.Service.Repositories;

namespace savings_sage.Service.Authentication;

public class AuthenticationSeeder
{
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<User> userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly SavingsSageContext _context;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        IConfiguration configuration, ICategoryRepository categoryRepository, SavingsSageContext context)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        _configuration = configuration;
        _categoryRepository = categoryRepository;
        _context = context;
    }

    public async Task AddRoles()
    {
        var tAdmin = CreateAdminRole(roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(roleManager);
        tUser.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        var adminRole = _configuration["Roles:Admin"];
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        var userRole = _configuration["Roles:User"];
        await roleManager.CreateAsync(new IdentityRole(userRole));
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExsists();
        tAdmin.Wait();
    }

    public void AddAdminCategories()
    {
        var tAddCatAdmin = AddCategoryToAdmin();
        tAddCatAdmin.Wait();
    }

    private async Task CreateAdminIfNotExsists()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new User { UserName = "admin", Email = "admin@admin.com" };
            var password = _configuration["Secret:AdminPW"];
            var adminCreated = await userManager.CreateAsync(admin, password);

            if (adminCreated.Succeeded)
            {
                var adminRole = _configuration["Roles:Admin"];
                await userManager.AddToRoleAsync(admin, adminRole);
            }
        }
    }

    private async Task AddCategoryToAdmin()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        var toCheck = _context.Categories.Any();
        if (toCheck == false)
        {
            await _categoryRepository.CreateDefaultCategoriesAsync(adminInDb.Id);
        }
    }
}