using Microsoft.EntityFrameworkCore;
using savings_sage.Context;
using savings_sage.Model.Accounts;

namespace savings_sage.Service.Repositories;

public class AccountRepository(UsersContext context) : IAccountRepository
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

}