using savings_sage.Model.UserJoins;

namespace savings_sage.Model;

public class Budget
{ 
    public int Id { get; init; }
    public string Name { get; set; }
    public double Amount { get; set; } //current spending
    public double Threshold { get; set; } 
    
    public int OwnerId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
    public int CategoryId { get; set; } //ensuring the foreign key
    public Category Category { get; init; }
    public Currency Currency { get; init; }
    
    public bool GroupSharing { get; set; } = false;
    //connector table
    public ICollection<UserBudget> UserBudget { get; set; }
}