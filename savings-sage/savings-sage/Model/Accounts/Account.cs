
using savings_sage.Model.UserJoins;

namespace savings_sage.Model.Accounts;

public class Account
{
    public int Id { get; init; }
    public string Name { get; set; }
    public Currency Currency { get; init; }
    public string OwnerId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
    public double Amount { get; set; }
    public int? ParentAccountId { get; init; }
    public Account ParentAccount { get; init; }
    public ICollection<Account> SubAccounts { get; set; } = new List<Account>();
    public bool GroupSharingOption { get; set; }

    public bool CanGoMinus { get; set; } = false;
    public DateTime? ExpirationDate { get; set; } = null;
    public AccountType Type { get; set; }
    
    //connector table
    public ICollection<UserAccount> UserAccounts { get; set; } 
}