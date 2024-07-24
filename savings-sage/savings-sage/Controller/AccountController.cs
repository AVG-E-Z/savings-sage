using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Model.UserJoins;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository, IConfiguration configuration, UserManager<User> userManager)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpGet("All/u/{userName}/ha/IdList")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<int>>> GetByUserId(
        [FromRoute]string userName)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userAccounts = await _accountRepository.GetAllIdsByUser(user);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            const string message = "Error getting accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    [HttpGet("All/u/{userName}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByOwnerId(
        [FromRoute]string userName)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userId = user.Id;

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

    [HttpGet("All/u/r/{userName}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByReaderId(
        [FromRoute]string userName)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var userAccountsReadAccess = await _accountRepository.GetAllByReader(user);
            return Ok(userAccountsReadAccess);
        }
        catch (Exception e)
        {
            const string message = "Error getting reader permission accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpGet("All/u/w/{userName}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByWriterId(
        [FromRoute]string userName)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var userAccountsWriterAccess = await _accountRepository.GetAllByWriter(user);
            return Ok(userAccountsWriterAccess);
        }
        catch (Exception e)
        {
            const string message = "Error getting reader permission accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("u/{userName}/a/{id:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetById(
        [FromRoute]string userName,
        [FromRoute]int id)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var userAccount = await _accountRepository.GetByIdAsync(id);
            
            if (userAccount == null)
            {
                return NotFound("Account not found.");
            }
            
            if (user.Id != userAccount.OwnerId)
            {
                    return Unauthorized("You do not have access.");
            }
            
            return Ok(userAccount);
        }
        catch (Exception e)
        {
            var message = "Error getting sub accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpGet("u/{userName}/a/{id:int}/children")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByIdAllChildren(
        [FromRoute]string userName,
        [FromRoute]int id)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userAccounts = await _accountRepository.GetAllSubAccounts(id);
            
            if (userAccounts == null)
            {
                return NotFound("Account not found.");
            }

            if (user.Id != userAccounts.First().OwnerId)
            {
                return Unauthorized("You do not have access.");
            }
            
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            var message = "Error getting sub accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpPost("u/{userName}/Add")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> CreateNewAccount(
        [FromRoute]string userName,
        [FromBody] AccountDataBody accountDataBody)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            bool hasParent = false;
            Account parentAccount = new Account();
            if (accountDataBody.ParentAccountId.HasValue)
            {
                int pAId = accountDataBody.ParentAccountId.Value;
                parentAccount = await _accountRepository.GetByIdAsync(pAId);
                hasParent = true;
            }

            string ownerId = hasParent ? parentAccount.OwnerId : user.Id;
            
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

    [HttpDelete("u/{userName}/a/{id:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> DeleteAccountAndSubAccounts(
        [FromRoute]string userName,
        [FromRoute]int id)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return NotFound($"Account with {id} not found.");

            if (account.OwnerId != user.Id)
            {
                _logger.LogError($"Deletion attempt by {user.Id} - not account owner.");
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

    [HttpPut("u/{userName}/a/{id:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> UpdateAccount(
        [FromRoute]string userName,
        [FromRoute]int id,
        [FromBody] AccountDataBody accountDataBody)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return NotFound($"Account with {id} not found.");

            if (account.OwnerId != user.Id)
            {
                _logger.LogError($"Update attempt by {user.Id} - not account owner.");
                return Forbid();
            }
            
            //todo amount adjusting transaction front or backend
            account.Name = accountDataBody.Name;
            account.GroupSharingOption = accountDataBody.GroupSharingOption;
            account.CanGoMinus = accountDataBody.CanGoMinus;
            account.ExpirationDate = accountDataBody.ExpirationDate;
            
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

    [HttpPut("u/{userName}/a/{id:int}/g/addmembers")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> UpdateAccountMembers(
        [FromRoute]string userName,
        [FromRoute]int id,
        [FromBody] UserAccountDataBody userAccountDataBody)
    {
        try
        {
            User owner = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (owner == null)
            {
                return BadRequest("Owner not found.");
            }
            
            User userToManage = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userAccountDataBody.UserName);
            if (userToManage == null)
            {
                return BadRequest("User not found.");
            }
    
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return NotFound($"Account with {id} not found.");

            if (account.OwnerId != owner.Id)
            {
                _logger.LogError($"Update attempt by {owner.Id} - not account owner.");
                return Forbid();
            }
            
            await _accountRepository.ManageMembers(account, userToManage, userAccountDataBody);
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
                    Name = $"{accountData.Name} (Interest)",
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
                    Name = $"{accountData.Name} (Capital)",
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