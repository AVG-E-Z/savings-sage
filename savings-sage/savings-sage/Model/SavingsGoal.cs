namespace savings_sage.Model;

public class SavingsGoal
{
    public int Id { get; init; }
    public string Name { get; set; }
    public double Amount { get; set; }
    
    public int UserId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
    public int CategoryId { get; set; } //ensuring the foreign key
    public Category Category { get; init; }
    
    public Currency Currency { get; init; }
    public double Goal { get; set; }
    
    public bool GroupSharing { get; set; } = false;
    public ICollection<User>? Writers { get; set; } = null;
    public ICollection<User>? Readers { get; set; } = null;
}