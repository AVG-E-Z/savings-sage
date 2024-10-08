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
    private readonly UserManager<User> _userManager;
    

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository, UserManager<User> userManager)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _userManager = userManager;
    }

    [HttpGet("All/u/owner/ha/IdList")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<int>>> GetByUserId()
    {
        try
        {
            var userName = User.Identity.Name;
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
    
    [HttpGet("All/u/owner")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByOwnerId()
    {
        try
        {
            var userName = User.Identity.Name;
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

    [HttpGet("All/u/reader")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByReaderId()
    {
        try
        {
            var userName = User.Identity.Name;
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
    
    [HttpGet("All/u/writer")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByWriterId()
    {
        try
        {
            var userName = User.Identity.Name;
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

    [HttpGet("u/a/{id:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetById(
        [FromRoute]int id)
    {
        try
        {
            var userName = User.Identity.Name;
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

    [HttpGet("u/a/{id:int}/children")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Account>>> GetByIdAllChildren(
        [FromRoute]int id)
    {
        try
        {
            var userName = User.Identity.Name;
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

    [HttpPost("u/Add")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> CreateNewAccount(
        [FromBody] AccountDataBody accountDataBody)
    {
        try
        {
            var userName = User.Identity.Name;
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
                "Debit" => AddDebitAccount(accountDataBody, ownerId),
                "Credit" => AddCreditAccount(accountDataBody, ownerId),
                "Loan" => AddLoanAccount(accountDataBody, ownerId),
                "Cash" => AddCashAccount(accountDataBody, ownerId),
                "Savings" => AddSavingsAccount(accountDataBody, ownerId),
                _ => throw new Exception("Invalid account type")
            };

            if (accountDataBody.ParentAccountId == null)
            {
                var newAccount = await _accountRepository.AddAsync(userAccount);return Ok(new { ok = true, account = newAccount });
            } else
            {
                var amendedParent = await _accountRepository.AddSubAsync(userAccount, parentAccount); return Ok(new { ok = true, account = amendedParent });
            }
            
        }
        catch (Exception e)
        {
            const string message = "An error occurred while processing your request.";
            _logger.LogError(e, message);
            return Problem(detail: message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("u/a/{id:int}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> DeleteAccountAndSubAccounts(
        [FromRoute]int id)
    {
        try
        {
            var userName = User.Identity.Name;
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
            return StatusCode(204);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while attempting to delete the account.");
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpPut("u/a/{id:int}/edit")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> UpdateAccount(
        [FromRoute]int id,
        [FromBody] AccountDataBody accountDataBody)
    {
        try
        {
            var userName = User.Identity.Name;
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

    [HttpPut("u/a/{id:int}/g/addmembers")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Account>> UpdateAccountMembers(
        [FromRoute]int id,
        [FromBody] UserAccountDataBody userAccountDataBody)
    {
        try
        {
            var userName = User.Identity.Name;
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