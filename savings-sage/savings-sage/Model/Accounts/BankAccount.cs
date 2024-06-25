
namespace savings_sage.Model.Accounts;

public abstract class BankAccount
{
    //tőketartozás
    //össztartozás
    //2 alszámla - ha mindkettőt megadod, külön vonja
    //ha egyiket, akkor össztartozás 
    
    public int Id { get; init; }
    public string Name { get; set; }
    public Currency Currency { get; init; }
    public User Owner { get; init; }
    public int? ParentAccountId { get; init; } = null;
    public bool GroupSharingOption { get; set; } = false;
    public IEnumerable<User>? Writers { get; set; } = null;
    public IEnumerable<User>? Readers { get; set; } = null;
    public bool CanGoMinus { get; set; } = false;
    public DateTime? ExpirationDate { get; init; } = null;
    public abstract AccountType Type { get; init; }
    
}