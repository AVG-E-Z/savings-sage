using savings_sage.Model;

namespace savings_sage.Service.Repositories;

public interface ITransactionRepository
{
    public Task<IEnumerable<Transaction>> GetAll(int loggedInUserId);
    public Task<IEnumerable<Transaction>> GetAllByAccount(int accountId);
    public Task AddNewTransaction(Transaction transaction);
    public Task<int?> DeleteTransaction(int id);
}