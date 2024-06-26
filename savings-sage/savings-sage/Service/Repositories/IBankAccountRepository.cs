using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public interface IBankAccountRepository
{
    Task<IEnumerable<BankAccount>> GetAll();
    Task<BankAccount?> GetAllById(int id);
    Task<IEnumerable<BankAccount>> GetAllByUser(int userId);
    Task<IEnumerable<BankAccount>> GetAllByUserByType(int userId, AccountType type);
    Task<IEnumerable<BankAccount>> GetAllSubAccounts(int accountId);
    Task Add(BankAccountDataBody accountData);
    Task DeleteWithSubAccounts(BankAccount account);
    Task Update(BankAccount account);
}