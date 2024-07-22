namespace savings_sage.Model;

public class Category
{
    public int Id { get; init; }
    
    public string OwnerId { get; set; } //ensuring the foreign key is the UserId
    public User Owner { get; init; }
    public string Name { get; set; }
    
    public int ColorId { get; set; }
    public Color Color { get; set; }
    //dream: icons public int IconId { get; set; }
}