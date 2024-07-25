using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using Microsoft.Identity.Client;
using savings_sage.Context;

namespace savings_sage.Service.Repositories;

public class TransactionRepository(UsersContext context) : ITransactionRepository
{
    // public async Task<IEnumerable<Transaction>> GetAllByOwner(string loggedInUserId)
    // {
    //     var allTransactions = await context.Transactions.Where(transaction => transaction.OwnerId == loggedInUserId).ToListAsync();
    //
    //     return allTransactions;
    // }
    
    
    public async Task<IEnumerable<Transaction>> GetAllByAccount(int accountId)
    {
        var allTransactionsByAccount = await context.Transactions.Where(transaction => transaction.AccountId == accountId).ToListAsync();

        return allTransactionsByAccount;
    }

    public async Task<IEnumerable<Transaction>> GetAllForAllAccounts(int[] accountIds)
    {
        var allTransactionsForAllAccounts = await context.Transactions.Where(transaction => accountIds.Contains(transaction.AccountId)).ToListAsync();

        return allTransactionsForAllAccounts;
    }

    public async Task AddNewTransaction(Transaction transaction)
    {
        Console.WriteLine("Saving new transaction to db...");
       await context.Transactions.AddAsync(transaction);
       await context.SaveChangesAsync();
    }

    public async Task<int?> DeleteTransaction(int id)
    {
        var transactionToRemove = await context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);

        if (transactionToRemove == null) return null;
        Console.WriteLine($"Removing transaction no. {id} from database...");
        context.Transactions.Remove(transactionToRemove);
        await context.SaveChangesAsync();
        return transactionToRemove.Id;
    }
}