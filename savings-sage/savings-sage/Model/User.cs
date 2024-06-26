using savings_sage.Model.Accounts;

namespace savings_sage.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    
    public ICollection<Group> Groups { get; set; }
    public ICollection<BankAccount> Accounts { get; set; }
    public ICollection<SavingsGoal> SavingsGoals { get; set; }
    public ICollection<Budget> Budgets { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Birthday { get; init; }
    //password?
}