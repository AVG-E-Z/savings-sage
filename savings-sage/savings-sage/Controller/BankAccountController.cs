using Microsoft.AspNetCore.Mvc;
using savings_sage.Model.Accounts;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly ILogger<BankAccountController> _logger;
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountController(ILogger<BankAccountController> logger, IBankAccountRepository bankAccountRepository)
    {
        _logger = logger;
        _bankAccountRepository = bankAccountRepository;
    }

    [HttpGet("BankAccounts/all")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetAllAccounts()
    {
        try
        {
            var allAccounts = await _bankAccountRepository.GetAll();
            return Ok(allAccounts);
        }
        catch (Exception e)
        {
            const string message = "Error getting accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpGet("BankAccounts/{id:int}")]
    public async Task<ActionResult<BankAccount>> GetById(int id)
    {
        try
        {
            var account = await _bankAccountRepository.GetById(id);
            return Ok(account);
        }
        catch (Exception e)
        {
            const string message = "Error getting account";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpGet("BankAccounts/User/{userId:int}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetByOwnerId(int userId)
    {
        try
        {
            var userAccounts = await _bankAccountRepository.GetAllByOwner(userId);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            const string message = "Error getting accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpGet("BankAccounts/User/{userId:int}/type/{type}")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetByOwnerIdByType(int userId, AccountType type)
    {
        try
        {
            var userAccounts = await _bankAccountRepository.GetAllByOwnerByType(userId, type);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            string message = $"Error getting {type} accounts of user";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpGet("BankAccounts/{id:int}/children")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetByIdAllChildren(int id)
    {
        try
        {
            var userAccounts = await _bankAccountRepository.GetAllSubAccounts(id);
            return Ok(userAccounts);
        }
        catch (Exception e)
        {
            string message = "Error getting sub accounts";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }
    
    [HttpPost("BankAccounts/User/{ownerId:int}/Create")]
    public async Task<ActionResult<BankAccount>> CreateNewAccount([FromBody] BankAccountDataBody accountDataBody, int ownerId)
    {
        try
        {
            BankAccount userAccount = accountDataBody.Type switch
            {
                AccountType.Debit => AddDebitAccount(accountDataBody, ownerId),
                AccountType.Credit => AddCreditAccount(accountDataBody, ownerId),
                AccountType.Loan => AddLoanAccount(accountDataBody, ownerId),
                AccountType.Cash => AddCashAccount(accountDataBody, ownerId),
                AccountType.Savings => AddSavingsAccount(accountDataBody, ownerId),
                _ => throw new Exception("Invalid account type")
            };

            var newAccount = await _bankAccountRepository.AddAsync(userAccount);
            

            // Assuming AddAsync method returns the added accounts
            if (newAccount != null)
            {
                // Use the first account to create the URI for CreatedAtAction
                return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
            }
            else
            {
                return BadRequest("Failed to create accounts");
            }
        }
        catch (Exception e)
        {
            const string message = "Error creating account";
            _logger.LogError(e, message);
            return NotFound(message);
        }
    }

    [HttpDelete("BankAccounts/User/{userId:int}/Account/{id:int}")]
    public async Task<ActionResult<BankAccount>> DeleteAccountAndSubAccounts(int userId, int id)
    {
         try
         {
             var account = await _bankAccountRepository.GetById(id);
             if (account == null)
             {
                 return NotFound($"Account with {id} not found.");
             }

             if (account.OwnerId != userId)
             {
                 _logger.LogError($"Deletion attempt by {userId} - not account owner.");
                 return Forbid();
             }

             await _bankAccountRepository.DeleteWithSubAccounts(account);
             return Ok(account);
         }
         catch (Exception e)
         {
             const string message = "Error deleting account";
             _logger.LogError(e, message);
             return NotFound(message);
         }
    }
    
    [HttpPut("BankAccounts/User/{userId:int}/Account/{id:int}")]
    public async Task<ActionResult<BankAccount>> UpdateAccount([FromBody] BankAccountDataBody accountDataBody, int userId, int id)
    {
        try
        {
            var account = await _bankAccountRepository.GetById(id);
            if (account == null)
            {
                return NotFound($"Account with {id} not found.");
            }

            if (account.OwnerId != userId)
            {
                _logger.LogError($"Update attempt by {userId} - not account owner.");
                return Forbid();
            }
            
            await _bankAccountRepository.Update(account);
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
    
    private BankAccount AddDebitAccount(BankAccountDataBody accountData, int ownerId)
    {
        BankAccount account = new BankAccount
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
    
    private BankAccount AddCreditAccount(BankAccountDataBody accountData, int ownerId)
    {
        BankAccount account = new BankAccount
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
    
    private BankAccount AddLoanAccount(BankAccountDataBody accountData, int ownerId)
    {
        BankAccount accountMain = new BankAccount
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
            SubAccounts = new List<BankAccount>{ 
                new BankAccount
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
                new BankAccount
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
    
    private BankAccount AddCashAccount(BankAccountDataBody accountData, int ownerId)
    {
        BankAccount account = new BankAccount
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
    
    private BankAccount AddSavingsAccount(BankAccountDataBody accountData, int ownerId)
    {
        BankAccount account = new BankAccount
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