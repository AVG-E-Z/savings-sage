namespace savings_sage.Model;

public class Group
{
    public int Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<User> Members { get; set; }
    public User Owner { get; init; }
}