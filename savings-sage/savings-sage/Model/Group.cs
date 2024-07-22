namespace savings_sage.Model;

public class Group
{
    public int Id { get; init; }
    public string Name { get; init; }
    public ICollection<User> Members { get; set; }
    
    public string OwnerId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
}