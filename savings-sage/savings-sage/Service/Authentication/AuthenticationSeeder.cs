using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Service.Repositories;

namespace savings_sage.Service.Authentication;

public class AuthenticationSeeder
{
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<User> userManager;
    private readonly ICategoryRepository _categoryRepository;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        IConfiguration configuration, ICategoryRepository categoryRepository)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        _configuration = configuration;
        _categoryRepository = categoryRepository;
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
            var adminCreated = await userManager.CreateAsync(admin, "admin123");

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
        await _categoryRepository.CreateDefaultCategoriesAsync(adminInDb.Id);
    }
}