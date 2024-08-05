
using savings_sage.Model;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public interface IAccountTransactionRepository
{
    Task<Account> UpdateAmount(int accId, Transaction transaction, int? siblingAccId);
}