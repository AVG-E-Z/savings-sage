namespace savings_sage.Model.UserJoins;

public class UserAccountDataBody
{
    public string UserName { get; set; }
    
    public int BackAccountId { get; set; }

    public bool IsReader { get; set; }
    public bool IsReaderAndWriter { get; set; }
}