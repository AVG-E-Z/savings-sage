namespace savings_sage.Model;

public class Transaction
{
    public int Id { get; init; }
    public string Name { get; set; }
    public Currency Currency { get; init; }
    public int UserId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
    public int AccountId { get; init; }
    public DateTime Date { get; init; }
    public int CategoryId { get; set; } //ensuring the foreign key
    public Category Category { get; set; }
    public double Amount { get; set; }
    public Direction Direction { get; set; }
    public bool IsRecurring { get; set; }
    public int RefreshDays { get; set; }
    public TransactionType Type { get; set; }
    public int? SiblingTransactionId { get; set; }
}