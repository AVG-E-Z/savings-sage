
namespace savings_sage.Model.Accounts;

public abstract class BankAccount
{
    protected BankAccount(string name, Currency currency, User owner, int? parentAccountId)
    {
        Name = name;
        Currency = currency;
        Owner = owner;
        ParentAccountId = parentAccountId;
    }

    public int Id { get; init; }
    public string Name { get; set; }
    public Currency Currency { get; init; }
    public User Owner { get; init; }
    public int? ParentAccountId { get; init; }
    public bool GroupSharingOption { get; set; } 
    public IEnumerable<User>? Writers { get; set; }
    public IEnumerable<User>? Readers { get; set; } 
    public abstract bool CanGoMinus { get;} 
    public abstract DateTime? ExpirationDate { get; } 
    public abstract AccountType Type { get;}
    
}