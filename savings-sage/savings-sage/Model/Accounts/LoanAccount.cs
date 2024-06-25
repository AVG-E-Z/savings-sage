namespace savings_sage.Model.Accounts;

public class LoanAccount(string name, Currency currency, User owner, int? parentAccountId, DateTime expiration)
    : BankAccount(name, currency, owner, parentAccountId)
{
    public override AccountType Type => AccountType.Loan;
    public override bool CanGoMinus => true;
    public override DateTime? ExpirationDate { get; } = expiration;
}