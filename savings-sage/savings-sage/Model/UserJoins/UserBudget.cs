namespace savings_sage.Model.UserJoins;

public class UserBudget
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int BudgetId { get; set; }
    public Budget Budget { get; set; }

    public bool IsReader { get; set; } = false;
    public bool IsWriter { get; set; } = false;
}