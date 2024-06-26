using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;
using DotNetEnv;

namespace savings_sage.Context;

public class SavingsSageContext : DbContext
{
    private IConfiguration _configuration;
    private string _password;
    
    public DbSet<BankAccount> Accounts { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<SavingsGoal> SavingsGoals { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> User { get; set; }
    
    public DbSet<UserBankAccount> UserBankAccounts { get; set; }
    public DbSet<UserBudget> UserBudgets { get; set; }
    public DbSet<UserSavingsGoal> UserSavingsGoals { get; set; }
    

    public SavingsSageContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Env.Load();
        _password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer($"Server=localhost,1433;Database=SavingsSage;User Id=sa;Password={_password};Encrypt=false;");
        //var connectionString = _configuration.GetConnectionString("Default");
        //optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Accounts
        
        modelBuilder.Entity<BankAccount>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<BankAccount>()
            .Property(x => x.Type)
            .HasConversion<string>();
        
        modelBuilder.Entity<BankAccount>()
            .Property(x => x.Currency)
            .HasConversion<string>();
        
        modelBuilder.Entity<BankAccount>()
            .HasIndex(b => new { b.OwnerId, b.Name })
            .IsUnique()
            .HasDatabaseName("IX_User_BankAccountName_Unique");
        
        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.Owner)
            .WithMany(u => u.BankAccounts)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion

        #region Budgets
        
        modelBuilder.Entity<Budget>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Budget>()
            .Property(x => x.Currency)
            .HasConversion<string>();
        
        modelBuilder.Entity<Budget>()
            .HasIndex(b => new { b.OwnerId, b.Name })
            .IsUnique()
            .HasDatabaseName("IX_User_BudgetName_Unique");
        
        modelBuilder.Entity<Budget>()
            .HasOne(b => b.Owner)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion

        #region Category
        
        modelBuilder.Entity<Category>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Category>()
            .HasIndex(b => new { b.OwnerId, b.Name })
            .IsUnique()
            .HasDatabaseName("IX_User_CategoryName_Unique");
        
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.Categories)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion

        #region Color
        
        modelBuilder.Entity<Color>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Color>()
            .HasIndex(x => x.HexadecimalCode)
            .IsUnique();
        
        modelBuilder.Entity<Color>()
            .HasIndex(x => x.ClassNameColor)
            .IsUnique();
        
        modelBuilder.Entity<Color>()
            .HasIndex(x => x.Name)
            .IsUnique();
        
        #endregion

        #region Group
        
        modelBuilder.Entity<Group>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Group>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.Groups)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Group>()
            .HasIndex(g => new { g.OwnerId, g.Name })
            .IsUnique()
            .HasDatabaseName("IX_User_GroupyName_Unique");
        
        #endregion
        
        #region SavingsGoal
        
        modelBuilder.Entity<SavingsGoal>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<SavingsGoal>()
            .Property(x => x.Currency)
            .HasConversion<string>();
        
        modelBuilder.Entity<SavingsGoal>()
            .HasOne(s => s.Owner)
            .WithMany(u => u.SavingsGoals)
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //If id is not the connection comment out this and add ICOllection<SavingsGoal> to Category
        // modelBuilder.Entity<SavingsGoal>()
        //     .HasOne(s => s.Category)
        //     .WithOne(x => x.SavingsGoals)
        //     .HasForeignKey(s => s.CategoryId); //comment out fk in SavingsGoal
        
        
        #endregion
        
        #region Transaction
        
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Currency)
            .HasConversion<string>();
        
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Direction)
            .HasConversion<string>();
        
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion

        #region Users
        
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<User>()
            .HasIndex(x => x.EmailAddress)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(x => x.UserName)
            .IsUnique();
        
        #endregion

        #region Connector tables

        //Bank account
        modelBuilder.Entity<UserBankAccount>()
            .HasKey(uba => new { uba.UserId, uba.BackAccountId });

        modelBuilder.Entity<UserBankAccount>()
            .HasOne(uba => uba.User)
            .WithMany(u => u.UserBankAccount)
            .HasForeignKey(uba => uba.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserBankAccount>()
            .HasOne(uba => uba.BankAccount)
            .WithMany(ba => ba.UserBankAccount)
            .HasForeignKey(uba => uba.BackAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        //Budget
        modelBuilder.Entity<UserBudget>()
            .HasKey(ub => new { ub.UserId, ub.BudgetId });

        modelBuilder.Entity<UserBudget>()
            .HasOne(ub => ub.User)
            .WithMany(u => u.UserBudget)
            .HasForeignKey(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserBudget>()
            .HasOne(ub => ub.Budget)
            .WithMany(b => b.UserBudget)
            .HasForeignKey(ub => ub.BudgetId)
            .OnDelete(DeleteBehavior.Restrict);

        //Savings
        modelBuilder.Entity<UserSavingsGoal>()
            .HasKey(usg => new { usg.UserId, usg.SavingsGoalId });

        modelBuilder.Entity<UserSavingsGoal>()
            .HasOne(usg => usg.User)
            .WithMany(u => u.UserSavingsGoal)
            .HasForeignKey(usg => usg.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserSavingsGoal>()
            .HasOne(usg => usg.SavingsGoal)
            .WithMany(sg => sg.UserSavingsGoal)
            .HasForeignKey(usg => usg.SavingsGoalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }
}