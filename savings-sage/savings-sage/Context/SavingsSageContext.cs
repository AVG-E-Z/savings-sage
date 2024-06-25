using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Model.Accounts;

namespace savings_sage.Context;

public class SavingsSageContext : DbContext
{
    private IConfiguration _configuration;
    
    public DbSet<BankAccount> Accounts { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<SavingsGoal> SavingsGoals { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    //public DbSet<User> User { get; set; }
    

    public SavingsSageContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Ant34teR;Encrypt=false;");
        var connectionString = _configuration.GetConnectionString("Default");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Accounts
        modelBuilder.Entity<BankAccount>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region Budgets
        modelBuilder.Entity<Budget>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region Category
        modelBuilder.Entity<Category>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Category>()
            .HasIndex(x => x.Name)
            .IsUnique();
        #endregion

        #region Color
        modelBuilder.Entity<Color>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Color>()
            .HasIndex(x => x.HexadecimalCode)
            .IsUnique();
        #endregion

        #region Group
        modelBuilder.Entity<Group>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Group>()
            .HasIndex(x => x.Name)
            .IsUnique();
        #endregion
        
        #region SavingsGoal
        modelBuilder.Entity<SavingsGoal>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        //todo ez kell? -Zs
        modelBuilder.Entity<SavingsGoal>()
            .HasIndex(x => x.Category)
            .IsUnique();
        #endregion
        
        #region Transaction
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region Users
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<User>()
            .HasIndex(x => x.EmailAddress)
            .IsUnique();
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }
}