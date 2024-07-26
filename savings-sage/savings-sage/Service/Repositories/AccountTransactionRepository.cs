using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public class AccountTransactionRepository (UsersContext context) : IAccountTransactionRepository
{
    
    public async Task<Account> UpdateAmount(int accId, Transaction transaction, int? siblingAccId)
    {
        var accountToAmend = await context.Accounts.FirstOrDefaultAsync(x => accId == x.Id);
        
        switch (transaction.Type)
        {
            case TransactionType.Payment:
                if (transaction.Direction == Direction.In)
                {
                    accountToAmend.Amount =+ transaction.Amount;
                }
                accountToAmend.Amount =- transaction.Amount;
                break;
            case TransactionType.Correction:
                accountToAmend.Amount = transaction.Amount;
                break;
            case TransactionType.Transfer:
                var siblingAccToAmend = await context.Accounts.FirstOrDefaultAsync(x => siblingAccId == x.Id);
                if (transaction.Direction == Direction.In)
                {
                    accountToAmend.Amount =+ transaction.Amount;
                    siblingAccToAmend.Amount =- transaction.Amount;
                }
                accountToAmend.Amount =- transaction.Amount;
                siblingAccToAmend.Amount =+ transaction.Amount;
                context.Accounts.Update(siblingAccToAmend);
                await context.SaveChangesAsync();
                break;
            case TransactionType.Exchange:
                throw new Exception("upcoming feature, not yet working");
            default:
                throw new Exception("Invalid transaction type.");
        }
        
        context.Accounts.Update(accountToAmend);
        await context.SaveChangesAsync();
        
        return accountToAmend; 
    }

}