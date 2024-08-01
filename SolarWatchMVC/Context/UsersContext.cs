using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Context;

public class UsersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    private IConfiguration _configuration;
    
    public UsersContext (DbContextOptions<UsersContext> options,IConfiguration configuration)
        : base(options)
    {
       // _configuration = configuration;
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer("Server=localhost,1433;Database=SolarApi;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=true;HostNameInCertificate=localhost;");
    // }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}