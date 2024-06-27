using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public interface IBankAccountRepository
{
    Task<IEnumerable<BankAccount>> GetAll();
    Task<BankAccount?> GetById(int id);
    Task<IEnumerable<BankAccount>> GetAllByOwner(int userId);
    Task<IEnumerable<BankAccount>> GetAllByOwnerByType(int userId, AccountType type);
    Task<IEnumerable<BankAccount>> GetAllSubAccounts(int accountId);
    Task<BankAccount> AddAsync(BankAccount account);
    Task DeleteWithSubAccounts(BankAccount account);
    Task Update(BankAccount account);
}