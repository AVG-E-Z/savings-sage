namespace SavingsSage.Model;

public class BankAccount
{
    //tőketartozás
    //össztartozás
    //2 alszámla - ha mindkettőt megadod, külön vonja
    //ha egyiket, akkor össztartozás 
    
    public int Id { get; init; }
    public string Name { get; set; }
    public AccountType Type { get; init; }
}