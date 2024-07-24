namespace savings_sage.Model.Accounts;

public class AccountDataBody
{
    public string Name { get; set; }
    public Currency Currency { get; set; }
    public double Amount { get; set; }
    public double? AmountInterest { get; set; }
    public double? AmountCapital { get; set; }
    public int? ParentAccountId { get; init; }
    public bool GroupSharingOption { get; set; }
    public bool CanGoMinus { get; set; }
    public DateTime? ExpirationDate { get; set; } = null;
    public AccountType Type { get; set; }
}