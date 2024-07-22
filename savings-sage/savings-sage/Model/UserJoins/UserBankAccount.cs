using savings_sage.Model.Accounts;

namespace savings_sage.Model.UserJoins;

public class UserBankAccount
{
    public string UserId { get; set; }
    public User User { get; set; }
    
    public int BackAccountId { get; set; }
    public BankAccount BankAccount { get; set; }

    public bool IsReader { get; set; } = false;
    public bool IsWriter { get; set; } = false;
}