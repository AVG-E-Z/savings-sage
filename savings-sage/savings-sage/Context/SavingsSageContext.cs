using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;

namespace savings_sage.Context;

public class SavingsSageContext : IdentityDbContext<User, IdentityRole, string>
{
        public SavingsSageContext(DbContextOptions<SavingsSageContext> options, IConfiguration configuration)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SavingsGoal> SavingsGoals { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserBudget> UserBudgets { get; set; }
        public DbSet<UserSavingsGoal> UserSavingsGoals { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        #region Accounts
        
        modelBuilder.Entity<Account>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Account>()
            .HasKey(ba => ba.Id);
        
        modelBuilder.Entity<Account>()
            .Property(x => x.Type)
            .HasConversion<string>();
        
        modelBuilder.Entity<Account>()
            .Property(x => x.Currency)
            .HasConversion<string>();
        
        modelBuilder.Entity<Account>()
            .HasIndex(b => new { b.OwnerId, b.Name })
            .IsUnique()
            .HasDatabaseName("IX_User_AccountName_Unique");
        
        modelBuilder.Entity<Account>()
            .HasOne(ba => ba.Owner)
            .WithMany(u => u.Accounts)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Account>()
            .HasOne(ba => ba.ParentAccount)
            .WithMany(ba => ba.SubAccounts)
            .HasForeignKey(ba => ba.ParentAccountId)
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

        modelBuilder.Entity<Category>()
            .Property(c => c.IconURL)
            .IsRequired(true)
            .HasMaxLength(200)
            .HasColumnName("IconURL");
        
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
        //
        // #region Users
        //
        // modelBuilder.Entity<User>()
        //     .Property(x => x.Id)
        //     .ValueGeneratedOnAdd();
        //
        // modelBuilder.Entity<User>()
        //     .HasIndex(x => x.Email)
        //     .IsUnique();
        //
        // modelBuilder.Entity<User>()
        //     .HasIndex(x => x.UserName)
        //     .IsUnique();
        //
        // #endregion

        #region Connector tables

        //Bank account
        modelBuilder.Entity<UserAccount>()
            .HasKey(uba => new { uba.UserId, uba.BackAccountId });

        modelBuilder.Entity<UserAccount>()
            .HasOne(uba => uba.User)
            .WithMany(u => u.UserAccounts)
            .HasForeignKey(uba => uba.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserAccount>()
            .HasOne(uba => uba.Account)
            .WithMany(ba => ba.UserAccounts)
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

        #region data seeding

        modelBuilder.Entity<Color>().HasData(
            new Color {Id = 1, Name = "donatelloPurple", ClassNameColor = "purpleColor", HexadecimalCode = "A568CB"},
            new Color {Id = 2, Name = "pantherPink", ClassNameColor = "lightpinkColor", HexadecimalCode = "FF96B0"},
            new Color {Id = 3, Name = "flakyRed", ClassNameColor = "redColor", HexadecimalCode = "D3265D"},
            new Color {Id = 4, Name = "dextersHairOrange", ClassNameColor = "orangeColor", HexadecimalCode = "EA7417"},
            new Color {Id = 5, Name = "topCatYellow", ClassNameColor = "yellowColor", HexadecimalCode = "FEE411"},
            new Color {Id = 6, Name = "buttercupGreen", ClassNameColor = "lightGreenColor", HexadecimalCode = "66CE3C"},
            new Color {Id = 7, Name = "tophGreen", ClassNameColor = "greenColor", HexadecimalCode = "2AA168"},
            new Color {Id = 8, Name = "bubblesBlue", ClassNameColor = "lightBlueColor", HexadecimalCode = "56DBEF"},
            new Color {Id = 9, Name = "johhnysJeansBlue", ClassNameColor = "blueColor", HexadecimalCode = "3598FE"},
            new Color {Id = 10, Name = "sailorMercuryBlue", ClassNameColor = "navyBlueColor", HexadecimalCode = "5662CB"},
            new Color {Id = 11, Name = "scoobyDooBrown", ClassNameColor = "brownColor", HexadecimalCode = "98755B"},
            new Color {Id = 12, Name = "astroGray", ClassNameColor = "grayColor", HexadecimalCode = "979C98"}
            );
        

        #endregion


        }
}