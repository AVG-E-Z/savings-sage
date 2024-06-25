namespace savings_sage.Model.Accounts;

public class SavingsAccount(string name, Currency currency, User owner, int? parentAccountId, DateTime expiration)
    : BankAccount(name, currency, owner, parentAccountId)
{
    public override bool CanGoMinus => false; //vagy??
    public override DateTime? ExpirationDate => expiration;
    public override AccountType Type => AccountType.Savings;
}