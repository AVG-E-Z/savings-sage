namespace SavingsSage.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    public IEnumerable<Group> Groups { get; set; }
    public IEnumerable<BankAccount> Accounts { get; set; }
    public IEnumerable<SavingsGoal> SavingsGoals { get; set; }
    public IEnumerable<Budget> Budgets { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Birthday { get; init; }
    
}