using savings_sage.Context;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public class BankAccountRepository(SavingsSageContext context) : IBankAccountRepository
{
    public IEnumerable<BankAccount> GetAll()
    {
        return context.Accounts.ToList();
    }
    
    
    public IEnumerable<BankAccount> GetAllByUser(int UserId)
    {
        return context.Accounts.Where(a => a.OwnerId == UserId).ToList();
    }
    
}