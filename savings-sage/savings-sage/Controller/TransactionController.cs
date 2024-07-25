using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using savings_sage.Model;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly UserManager<User> _userManager;

    public TransactionController(
        ILogger<TransactionController> logger, 
        ITransactionRepository transactionRepository,  
        UserManager<User> userManager)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _userManager = userManager;
    }
    
    // [HttpGet("GetAllByOwner/{userName}")]
    // public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions([Required] string loggedInUserId)
    // {
    //     try
    //     {
    //         var allTransactions = await transactionRepository.GetAllByOwner(loggedInUserId);
    //         return Ok(allTransactions);
    //     }
    //     catch (ArgumentException e)
    //     {
    //         logger.LogError(e, "An error occured, while fetching transactions for user. Check if there is a valid logged in user.");
    //         return BadRequest();
    //     }
    //     catch (Exception e)
    //     {
    //         logger.LogError(e, "An error occured while fetching transactions.");
    //         return NotFound();
    //     }
    // }

    [HttpPost("GetAll/ForAllUserAccounts")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllForAllUserAccounts([Required] int[] accountIds)
    {
        try
        {
            _logger.LogInformation("Fetching transactions for all accounts...");
            var allTransactionForAllAccounts = _transactionRepository.GetAllForAllAccounts(accountIds);
            return Ok(allTransactionForAllAccounts);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while fetching transactions for account.");
            return NotFound();
        }
    }
    
    [HttpGet("GetAll/Account/{accountId}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllForAccount([Required] int accountId)
    {
        try
        {
            _logger.LogInformation("Fetching transactions for account...");
            var allTransactionsForAccount = await _transactionRepository.GetAllByAccount(accountId);
            return Ok(allTransactionsForAccount);
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, "An error occured, while fetching transactions for account. Check if there is an account.");
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while fetching transactions for account.");
            return NotFound();
        }
    }
    
    
    [HttpPost("Add/{userName}")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public async Task<ActionResult<Transaction>> AddNewTransaction([FromBody] TransactionBody transactionBody, [FromRoute]string userName)
    {
        Console.WriteLine("hi");
        try
        { 
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            Console.WriteLine(user.UserName);
            
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var transactionToAdd = new Transaction
            {
                Name = transactionBody.Name,
                AccountId = transactionBody.AccountId,
                Amount = transactionBody.Amount,
                CategoryId = transactionBody.CategoryId,
                Currency = transactionBody.Currency,
                OwnerId = user.Id,
                Date = transactionBody.Date,
                Direction = transactionBody.Direction,
                IsRecurring = transactionBody.IsRecurring,
                RefreshDays = transactionBody.RefreshDays,
                Type = transactionBody.Type,
                SiblingTransactionId = transactionBody.SiblingTransactionId
            };

            await _transactionRepository.AddNewTransaction(transactionToAdd);
            return Ok(transactionToAdd);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while adding the transaction to the database");
            return NotFound();
        }
        
    }

    [HttpDelete("Delete/{transactionId}")]
    public async Task<ActionResult> DeleteTransaction(int transactionId)
    {
        try
        {
            await _transactionRepository.DeleteTransaction(transactionId);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, "An error occured while deleting the transaction from the db. Please check transaction id.");
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while deleting the transaction from the db.");
            return NotFound();
        }
    }
}
