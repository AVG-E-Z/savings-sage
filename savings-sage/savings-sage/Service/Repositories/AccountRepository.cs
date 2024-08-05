using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;

namespace savings_sage.Service.Repositories;

public class AccountRepository(SavingsSageContext context) : IAccountRepository
{
    public async Task<IEnumerable<Account>> GetAll()
    {
        return await context.Accounts.ToListAsync();
    }
    
    public async Task<Account?> GetByIdAsync(int id)
    {
        return await context.Accounts.FindAsync(id);
    }

    public async Task<IEnumerable<Account>> GetAllByOwner(string userId)
    {
        return await context.Accounts.Where(a => a.OwnerId == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<int>> GetAllIdsByUser(User user)
    {
        List<int> idList = new List<int>();
        var ownedAccounts = await context.Accounts
            .Where(a => a.OwnerId == user.Id)
            .Select(a => a.Id)
            .ToListAsync();
        var readerAndWriterAccessUserAccounts = await context.UserAccounts.Where(ua => ua.UserId == user.Id && (ua.IsReader || ua.IsReaderAndWriter))
            .Select(a => a.BackAccountId)
            .ToListAsync();
        idList.AddRange(ownedAccounts);
        idList.AddRange(readerAndWriterAccessUserAccounts);
        return idList;
    }

    public async Task<IEnumerable<Account>> GetAllByReader(User user)
    {
        var readerAccessUserAccounts = await context.UserAccounts.Where(ua => ua.UserId == user.Id && ua.IsReader)
            .Select(a => a.BackAccountId)
            .ToListAsync();
        
        List<Account> accountList = new List<Account>();
        
        foreach (var ua_AccountId in readerAccessUserAccounts)
        {
            var accountToAdd = await context.Accounts.FirstOrDefaultAsync(x => x.Id == ua_AccountId);
            accountList.Add(accountToAdd);
        }

        return accountList;
    }

    public async Task<IEnumerable<Account>> GetAllByWriter(User user)
    {
        var writerAccessUserAccounts = await context.UserAccounts.Where(ua => ua.UserId == user.Id && ua.IsReaderAndWriter)
            .Select(a => a.BackAccountId)
            .ToListAsync();
        
        List<Account> accountList = new List<Account>();
        
        foreach (var ua_AccountId in writerAccessUserAccounts)
        {
            var accountToAdd = await context.Accounts.FirstOrDefaultAsync(x => x.Id == ua_AccountId);
            accountList.Add(accountToAdd);
        }

        return accountList;
    }
    
    public async Task<IEnumerable<Account>> GetAllByOwnerByType(string userId, AccountType type)
    {
        return await context.Accounts.Where(a => a.OwnerId == userId && a.Type == type).ToListAsync();
    }

    public async Task<IEnumerable<Account>> GetAllSubAccounts(int accountId)
    {
        var allSubAccounts = new List<Account>();
        var visited = new HashSet<int>();

        await GetSubAccountsRecursive(accountId, allSubAccounts, visited);

        return allSubAccounts;
    }

    private async Task GetSubAccountsRecursive(int accountId, List<Account> allSubAccounts, HashSet<int> visited)
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

    public async Task<Account> AddAsync(Account account)
    {
        await context.AddAsync(account);
        await context.SaveChangesAsync();
        return account;
    }

    public async Task<Account> AddSubAsync(Account childAccount, Account parentAccount)
    {
        await context.AddAsync(childAccount);
        await context.SaveChangesAsync();
        return parentAccount;
    }

    public async Task DeleteWithSubAccounts(Account account)
    {
        var allSubAccounts = new List<Account>();
        var visited = new HashSet<int>();

        await GetSubAccountsRecursive(account.Id, allSubAccounts, visited);

        foreach (var sub in allSubAccounts)
        {
            context.Remove(sub);
        }

        context.Remove(account);
        await context.SaveChangesAsync();
    }
    
    public async Task Update(Account account)
    {
        context.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task ManageMembers(Account account, User userToManage, UserAccountDataBody dataBody)
    {
        var updatedUserAccount = new UserAccount
        {
            Account = account,
            BackAccountId = account.Id,
            User = userToManage,
            UserId = userToManage.Id,
            IsReader = dataBody.IsReader,
            IsReaderAndWriter = dataBody.IsReaderAndWriter
        };
        var userAccountToManage = await context.UserAccounts.FirstOrDefaultAsync(ua => ua.UserId == userToManage.Id);
        if (userAccountToManage == null)
        {
            await context.UserAccounts.AddAsync(updatedUserAccount);
            await context.SaveChangesAsync();
        }
        var userAccountInAccount = account.UserAccounts.FirstOrDefault(ua => ua.UserId == userToManage.Id);
        if (userAccountInAccount == null)
        {
            account.UserAccounts.Add(updatedUserAccount);
        }

        userAccountInAccount = updatedUserAccount;
        
        context.Update(account);
        await context.SaveChangesAsync();
        
        context.Update(userAccountToManage);
        await context.SaveChangesAsync();
    }
}