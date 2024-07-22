namespace savings_sage.Model.UserJoins;

public class UserSavingsGoal
{
    public string UserId { get; set; }
    public User User { get; set; }
    
    public int SavingsGoalId { get; set; }
    public SavingsGoal SavingsGoal { get; set; }

    public bool IsReader { get; set; } = false;
    public bool IsWriter { get; set; } = false;
}