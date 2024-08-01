using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarApiContext :DbContext
{
    private IConfiguration _configuration;
  
    public DbSet<City> Cities { get; set; }
    public DbSet<SolarData> SolarDatas { get; set; }
    
    public SolarApiContext(DbContextOptions<SolarApiContext> options,IConfiguration configuration)
        : base(options)
    {
        //_configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<SolarData>()
            .Property(sd => sd.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}