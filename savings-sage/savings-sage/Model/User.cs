
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;

namespace savings_sage.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Birthday { get; init; }
    
    //todo password
    
    public ICollection<Group> Groups { get; set; }
    public ICollection<SavingsGoal> SavingsGoals { get; set; }
    public ICollection<BankAccount> BankAccounts { get; set; }
    public ICollection<Budget> Budgets { get; set; }
    public ICollection<Transaction> Transactions { get; set; }

    
    //connector table
    public ICollection<UserBankAccount> UserBankAccount { get; set; } 
    public ICollection<UserSavingsGoal> UserSavingsGoal { get; set; }
    public ICollection<UserBudget> UserBudget { get; set; }
    public ICollection<Category> Categories { get; set; }
}