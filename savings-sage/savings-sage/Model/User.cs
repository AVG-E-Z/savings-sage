
using Microsoft.AspNetCore.Identity;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;

namespace savings_sage.Model;

public class User : IdentityUser
{
    //public string FullName { get; set; }
    public DateTime Birthday { get; init; }
    //public string Email { get; set; }
        
    // Custom relationships
    public ICollection<Group> Groups { get; set; }
    public ICollection<SavingsGoal> SavingsGoals { get; set; }
    public ICollection<Account> Accounts { get; set; }
    public ICollection<Budget> Budgets { get; set; }
    public ICollection<Transaction> Transactions { get; set; }

    // Connector table relationships
    public ICollection<UserAccount> UserAccounts { get; set; } 
    public ICollection<UserSavingsGoal> UserSavingsGoal { get; set; }
    public ICollection<UserBudget> UserBudget { get; set; }
    public ICollection<Category> Categories { get; set; }
}