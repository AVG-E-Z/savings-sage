namespace savings_sage.Model;

public class TransactionBody
{
    public string Name { get; set; }
    public Currency Currency { get; init; }
  
    public int AccountId { get; init; }
    public DateTime Date { get; init; }
    public int? CategoryId { get; set; }
    public double Amount { get; set; }
    public string Direction { get; set; }
    public bool IsRecurring { get; set; }
    public int? RefreshDays { get; set; }
    public string Type { get; set; }
    public int? SiblingTransactionId { get; set; }
}