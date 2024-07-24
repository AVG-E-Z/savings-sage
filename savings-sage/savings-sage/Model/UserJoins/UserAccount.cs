using savings_sage.Model.Accounts;

namespace savings_sage.Model.UserJoins;

public class UserAccount
{
    public string UserId { get; set; }
    public User User { get; set; }
    
    public int BackAccountId { get; set; }
    public Account Account { get; set; }

    public bool IsReader { get; set; } = false;
    public bool IsReaderAndWriter { get; set; } = false;
}