using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace savings_sage.Context;

public class UsersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    private readonly IConfiguration _configuration;

    public UsersContext(DbContextOptions<UsersContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     var connectionString = _configuration.GetConnectionString("Default");
    //     optionsBuilder.UseSqlServer(connectionString);
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}