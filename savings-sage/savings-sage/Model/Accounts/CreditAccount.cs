namespace savings_sage.Model.Accounts;

public class CreditAccount(string name, Currency currency, User owner, int? parentAccountId)
    : BankAccount(name, currency, owner, parentAccountId)
{
    public override bool CanGoMinus => true;
    public override DateTime? ExpirationDate => null; //vagy??
    public override AccountType Type => AccountType.Credit;
}