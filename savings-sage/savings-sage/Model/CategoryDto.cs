namespace savings_sage.Model;

public class CategoryDto
{
    public int Id { get; init; }
    public string OwnerId { get; set; }
    public string Name { get; set; }
    public int ColorId { get; set; }
    public string IconURL { get; set; }
}