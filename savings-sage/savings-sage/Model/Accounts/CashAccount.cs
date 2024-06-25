namespace savings_sage.Model.Accounts;

public class CashAccount(string name, Currency currency, User owner, int? parentAccountId)
    : BankAccount(name, currency, owner, parentAccountId)
{
    public override bool CanGoMinus => false;
    public override DateTime? ExpirationDate => null;
    public override AccountType Type => AccountType.Cash;
}