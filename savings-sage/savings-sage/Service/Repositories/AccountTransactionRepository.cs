using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public class AccountTransactionRepository (SavingsSageContext context) : IAccountTransactionRepository
{
    
    public async Task<Account> UpdateAmount(Transaction transaction)
    {
        var accountToAmend = await context.Accounts.FirstOrDefaultAsync(x => transaction.AccountId == x.Id);
        
        switch (transaction.Type)
        {
            case TransactionType.Payment:
                if (transaction.Direction == Direction.In)
                {
                    accountToAmend.Amount += transaction.Amount;
                }
                else
                {
                    accountToAmend.Amount -= transaction.Amount;
                }
                break;
            
            case TransactionType.Correction:
                accountToAmend.Amount = transaction.Amount;
                break;
            
            case TransactionType.Transfer:
                var siblingAccToAmend = await context.Accounts.FirstOrDefaultAsync(x => transaction.SiblingTransactionId == x.Id);

                if (siblingAccToAmend == null)
                {
                    throw new Exception("Sibling account not found.");
                }

                if (transaction.Direction == Direction.In)
                {
                    accountToAmend.Amount += transaction.Amount;
                    siblingAccToAmend.Amount -= transaction.Amount;
                }
                else if (transaction.Direction == Direction.Out)
                {
                    accountToAmend.Amount -= transaction.Amount;
                    siblingAccToAmend.Amount += transaction.Amount;
                }

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