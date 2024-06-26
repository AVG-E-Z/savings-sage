using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public class BankAccountRepository(SavingsSageContext context) : IBankAccountRepository
{
    public async Task<IEnumerable<BankAccount>> GetAll()
    {
        return await context.Accounts.ToListAsync();
    }
    
    public async Task<BankAccount?> GetAllById(int id)
    {
        return await context.Accounts.FindAsync(id);
    }
    
    public async Task<IEnumerable<BankAccount>> GetAllByUser(int userId)
    {
        return await context.Accounts.Where(a => a.OwnerId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<BankAccount>> GetAllByUserByType(int userId, AccountType type)
    {
        return await context.Accounts.Where(a => a.OwnerId == userId && a.Type == type).ToListAsync();
    }

    public async Task<IEnumerable<BankAccount>> GetAllSubAccounts(int accountId)
    {
        var allSubAccounts = new List<BankAccount>();
        var visited = new HashSet<int>();

        await GetSubAccountsRecursive(accountId, allSubAccounts, visited);

        return allSubAccounts;
    }

    private async Task GetSubAccountsRecursive(int accountId, List<BankAccount> allSubAccounts, HashSet<int> visited)
    {
        // Base case: if the account is already visited, return
        if (visited.Contains(accountId))
        {
            return;
        }

        // Mark this account as visited
        visited.Add(accountId);

        // Get direct child accounts
        var childAccounts = context.Accounts.Where(a => a.ParentAccountId == accountId).ToList();

        foreach (var childAccount in childAccounts)
        {
            // Add child account to the result list
            allSubAccounts.Add(childAccount);

            // Recursively call for each child account
            await GetSubAccountsRecursive(childAccount.Id, allSubAccounts, visited);
        }
    }

    public async Task Add(BankAccountDataBody accountData)
    {
        switch (accountData.Type)
        {
            case AccountType.Debit:
                await AddDebitAccount(accountData);
                break;
            case AccountType.Credit:
                await AddCreditAccount(accountData);
                break;
            case AccountType.Loan:
                await AddLoanAccount(accountData);
                break;
            case AccountType.Cash:
                await AddCashAccount(accountData);
                break;
            case AccountType.Savings:
                await AddSavingsAccount(accountData);
                break;
            default:
                throw new Exception("invalid account type");
        }
    }

    public async Task DeleteWithSubAccounts(BankAccount account)
    {
        var allSubAccounts = new List<BankAccount>();
        var visited = new HashSet<int>();

        await GetSubAccountsRecursive(account.Id, allSubAccounts, visited);

        foreach (var sub in allSubAccounts)
        {
            context.Remove(sub);
        }

        context.Remove(account);
        await context.SaveChangesAsync();
    }
    
    public async Task Update(BankAccount account)
    {
        context.Update(account);
        await context.SaveChangesAsync();
    }

    private async Task AddDebitAccount(BankAccountDataBody accountData)
    {
        BankAccount account = new BankAccount
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Debit
        };
        
        context.Add(account);
        await context.SaveChangesAsync();
    }
    
    private async Task AddCreditAccount(BankAccountDataBody accountData)
    {
        BankAccount account = new BankAccount
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = true,
            Type = AccountType.Credit
        };
        
        context.Add(account);
        await context.SaveChangesAsync();
    }
    private async Task AddLoanAccount(BankAccountDataBody accountData)
    {
        BankAccount accountMain = new BankAccount
        {
            Name = $"{accountData.Name} (Kamat)",
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Loan
        };
        
        context.Add(accountMain);
        await context.SaveChangesAsync();
        
        BankAccount accountInterest = new BankAccount
        {
            Name = $"{accountData.Name} (Kamat)",
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountMain.Id,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Loan
        };
        
        BankAccount accountCapital = new BankAccount
        {
            Name = $"{accountData.Name} (Tőke)",
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountMain.Id,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Loan
        };
        
        context.Add(accountInterest);
        context.Add(accountCapital);
        await context.SaveChangesAsync();
    }
    private async Task AddCashAccount(BankAccountDataBody accountData)
    {
        BankAccount account = new BankAccount
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Cash
        };
        
        context.Add(account);
        await context.SaveChangesAsync();
    }
    private async Task AddSavingsAccount(BankAccountDataBody accountData)
    {
        BankAccount account = new BankAccount
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = accountData.OwnerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            ExpirationDate = accountData.ExpirationDate,
            Type = AccountType.Cash
        };
        
        context.Add(account);
        await context.SaveChangesAsync();
    }
}