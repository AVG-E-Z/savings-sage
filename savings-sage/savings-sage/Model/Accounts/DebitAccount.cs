using savings_sage.Model;
using savings_sage.Model.Accounts;

class DebitAccount(string name, Currency currency, User owner, int? parentAccountId)
    : BankAccount(name, currency, owner, parentAccountId)
{
    public override AccountType Type => AccountType.Debit;
    public override bool CanGoMinus => false;
    public override DateTime? ExpirationDate => null;
}