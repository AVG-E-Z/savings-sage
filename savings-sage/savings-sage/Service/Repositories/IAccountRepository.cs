using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;

namespace savings_sage.Service.Repositories;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAll();
    Task<Account?> GetByIdAsync(int id);
    Task<IEnumerable<Account>> GetAllByOwner(string userId);
    Task<IEnumerable<int>> GetAllIdsByUser(User user);
    Task<IEnumerable<Account>> GetAllByReader(User user);
    Task<IEnumerable<Account>> GetAllByWriter(User user);
    Task<IEnumerable<Account>> GetAllByOwnerByType(string userId, AccountType type);
    Task<IEnumerable<Account>> GetAllSubAccounts(int accountId);
    Task<Account> AddAsync(Account account);
    Task<Account> AddSubAsync(Account childAccount, Account parentAccount);
    Task DeleteWithSubAccounts(Account account);
    Task Update(Account account);
    Task ManageMembers(Account account, User userToManage, UserAccountDataBody dataBody);
}