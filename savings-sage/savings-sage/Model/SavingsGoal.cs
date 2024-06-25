namespace savings_sage.Model;

public class SavingsGoal
{
    public int Id { get; init; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public User Owner { get; init; }
    public Category Category { get; init; }
    public Currency Currency { get; init; }
    public double Goal { get; set; }
    public bool GroupSharing { get; set; } = false;
    public IEnumerable<User>? Writers { get; set; } = null;
    public IEnumerable<User>? Readers { get; set; } = null;
}