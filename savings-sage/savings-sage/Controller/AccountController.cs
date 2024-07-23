using Microsoft.AspNetCore.Mvc;
using savings_sage.Model.Accounts;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    [HttpGet("Accounts/all")]
    public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
    {
        try
        {
            var allAccounts = await _accountRepository.GetAll();
            return Ok(allAccounts);
        }
        catch (Exception e)
        {
            const string message = "Error getting accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("Accounts/{id:int}")]
    public async Task<ActionResult<Account>> GetById(int id)
    {
        
        try
        {
            var account = await _accountRepository.GetById(id);
            return Ok(account);
        }
        catch (Exception e)
        {
            const string message = "Error getting account";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("Accounts/User/{userId}")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByOwnerId(string userId)
    {
        try
        {
            var userAccounts = await _accountRepository.GetAllByOwner(userId);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            const string message = "Error getting accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("Accounts/User/{userId}/type/{type}")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByOwnerIdByType(string userId, AccountType type)
    {
        try
        {
            var userAccounts = await _accountRepository.GetAllByOwnerByType(userId, type);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            var message = $"Error getting {type} accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("Accounts/{id:int}/children")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByIdAllChildren(int id)
    {
        try
        {
            var userAccounts = await _accountRepository.GetAllSubAccounts(id);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            var message = "Error getting sub accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpPost("Accounts/User/{ownerId}/Create")]
    public async Task<ActionResult<Account>> CreateNewAccount([FromBody] AccountDataBody accountDataBody,
        string ownerId)
    {
        try
        {
            var userAccount = accountDataBody.Type switch
            {
                AccountType.Debit => AddDebitAccount(accountDataBody, ownerId),
                AccountType.Credit => AddCreditAccount(accountDataBody, ownerId),
                AccountType.Loan => AddLoanAccount(accountDataBody, ownerId),
                AccountType.Cash => AddCashAccount(accountDataBody, ownerId),
                AccountType.Savings => AddSavingsAccount(accountDataBody, ownerId),
                _ => throw new Exception("Invalid account type")
            };

            var newAccount = await _accountRepository.AddAsync(userAccount);


            // Assuming AddAsync method returns the added accounts
            if (newAccount != null)
                // Use the first account to create the URI for CreatedAtAction
                return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
            return BadRequest("Failed to create accounts");
        }
        catch (Exception e)
        {
            const string message = "An error occurred while processing your request.";
            _logger.LogError(e, message);
            return Problem(detail: message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("Accounts/User/{userId}/Account/{id:int}")]
    public async Task<ActionResult<Account>> DeleteAccountAndSubAccounts(string userId, int id)
    {
        try
        {
            var account = await _accountRepository.GetById(id);
            if (account == null) return NotFound($"Account with {id} not found.");

            if (account.OwnerId != userId)
            {
                _logger.LogError($"Deletion attempt by {userId} - not account owner.");
                return Forbid();
            }

            await _accountRepository.DeleteWithSubAccounts(account);
            return Ok(account);
        }
        catch (Exception e)
        {
            const string message = "Error deleting account";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpPut("Accounts/User/{userId}/Account/{id:int}")]
    public async Task<ActionResult<Account>> UpdateAccount([FromBody] AccountDataBody accountDataBody,
        string userId, int id)
    {
        try
        {
            var account = await _accountRepository.GetById(id);
            if (account == null) return NotFound($"Account with {id} not found.");

            if (account.OwnerId != userId)
            {
                _logger.LogError($"Update attempt by {userId} - not account owner.");
                return Forbid();
            }

            await _accountRepository.Update(account);
            return Ok(account);
        }
        catch (Exception e)
        {
            const string message = "Error updating account";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    #region CreateAccounts

    private Account AddDebitAccount(AccountDataBody accountData, string ownerId)
    {
        var account = new Account
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = ownerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Debit
        };
        return account;
    }

    private Account AddCreditAccount(AccountDataBody accountData, string ownerId)
    {
        var account = new Account
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = ownerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = true,
            Type = AccountType.Credit
        };
        return account;
    }

    private Account AddLoanAccount(AccountDataBody accountData, string ownerId)
    {
        var accountMain = new Account
        {
            Name = $"{accountData.Name}",
            Currency = accountData.Currency,
            OwnerId = ownerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            ExpirationDate = accountData.ExpirationDate,
            Type = AccountType.Loan,
            SubAccounts = new List<Account>
            {
                new()
                {
                    Name = $"{accountData.Name} (Kamat)",
                    Currency = accountData.Currency,
                    OwnerId = ownerId,
                    Amount = accountData.AmountInterest ?? 0,
                    GroupSharingOption = accountData.GroupSharingOption,
                    CanGoMinus = false,
                    ExpirationDate = accountData.ExpirationDate,
                    Type = AccountType.Loan
                },
                new()
                {
                    Name = $"{accountData.Name} (Tőke)",
                    Currency = accountData.Currency,
                    OwnerId = ownerId,
                    Amount = accountData.AmountCapital ?? 0,
                    GroupSharingOption = accountData.GroupSharingOption,
                    CanGoMinus = false,
                    ExpirationDate = accountData.ExpirationDate,
                    Type = AccountType.Loan
                }
            }
        };
        return accountMain;
    }

    private Account AddCashAccount(AccountDataBody accountData, string ownerId)
    {
        var account = new Account
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = ownerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            Type = AccountType.Cash
        };
        return account;
    }

    private Account AddSavingsAccount(AccountDataBody accountData, string ownerId)
    {
        var account = new Account
        {
            Name = accountData.Name,
            Currency = accountData.Currency,
            OwnerId = ownerId,
            Amount = accountData.Amount,
            ParentAccountId = accountData.ParentAccountId,
            GroupSharingOption = accountData.GroupSharingOption,
            CanGoMinus = false,
            ExpirationDate = accountData.ExpirationDate,
            Type = AccountType.Cash
        };
        return account;
    }

    #endregion
}