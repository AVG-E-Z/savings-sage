
using savings_sage.Model.UserJoins;

namespace savings_sage.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public DateTime Birthday { get; init; }
    //password?
    
    public ICollection<Group> Groups { get; set; }
    
    //connector table
    public ICollection<UserBankAccount> UserBankAccount { get; set; } 
    public ICollection<UserSavingsGoal> UserSavingsGoal { get; set; }
    public ICollection<UserBudget> UserBudget { get; set; }
}