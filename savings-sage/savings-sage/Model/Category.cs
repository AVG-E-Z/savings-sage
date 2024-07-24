namespace savings_sage.Model;

public class Category
{
    public int Id { get; init; }
    
    public string OwnerId { get; set; }
    public User Owner { get; init; }
    public string Name { get; set; }
    
    public int ColorId { get; set; }
    public Color Color { get; set; }
    public string IconURL { get; set; }
}