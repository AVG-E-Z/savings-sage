namespace savings_sage.Model;

public class Color
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string HexadecimalCode { get; set; }
    public string ClassNameColor { get; set; }
    
    public int CategoryId { get; set; } //ensuring the foreign key
    public Category Category { get; set; } }