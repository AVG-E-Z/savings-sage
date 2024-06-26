namespace savings_sage.Model.Accounts;

public class BankAccountDataBody
{
    public string Name { get; set; }
    public Currency Currency { get; set; }
    public int OwnerId { get; set; }
    public double Amount { get; set; }
    public int? ParentAccountId { get; init; }
    public bool GroupSharingOption { get; set; }
    public DateTime? ExpirationDate { get; set; } = null;
    public AccountType Type { get; set; }
}