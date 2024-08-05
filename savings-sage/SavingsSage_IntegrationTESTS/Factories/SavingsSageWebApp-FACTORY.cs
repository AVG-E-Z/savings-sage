using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using savings_sage.Context;
using savings_sage.Model;

namespace SavingsSage_IntegrationTESTS.Factories;

public class SavingsSageWebApp_FACTORY : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
       builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SavingsSageContext>)));
            
            //Add new DbContextOptions for our two contexts, this time with in-memory db
            services.AddDbContext<SavingsSageContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            //We will need to initialize our in memory databases. 
            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();
              
            //We use this scope to request the registered dbcontexts, and initialize the schemas
            var savingsSageContext = scope.ServiceProvider.GetRequiredService<SavingsSageContext>();
            

            savingsSageContext.Database.EnsureDeleted();
            savingsSageContext.Database.EnsureCreated();

            // Seed Identity data
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
              
            // Add roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            // Add users
            var adminUser = new User { UserName = "admin", Email = "admin@admin.com" };
            var userCreationResult = userManager.CreateAsync(adminUser, "Password123!").GetAwaiter().GetResult();
              
            var normalUser = new User { UserName = "tesztelek", Email = "teszt@teszt.com" };
            var normalUserCreationResult = userManager.CreateAsync(normalUser, "asd123").GetAwaiter().GetResult();
            if (userCreationResult.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
            if (normalUserCreationResult.Succeeded)
            {
                userManager.AddToRoleAsync(normalUser, "User").GetAwaiter().GetResult();
            }
        });
    }

}