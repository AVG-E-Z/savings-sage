using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using savings_sage.Model;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ILogger<TransactionController> logger, ITransactionRepository transactionRepository)
    : ControllerBase
{
    
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions([Required] int loggedInUserId)
    {
        try
        {
            var allTransactions = await transactionRepository.GetAll(loggedInUserId);
            return Ok(allTransactions);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "An error occured, while fetching transactions for user. Check if there is a valid logged in user.");
            return BadRequest();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while fetching transactions.");
            return NotFound();
        }
    }
    
    [HttpGet("GetAll/Account/{accountId}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllForAccount([Required] int accountId)
    {
        try
        {
            logger.LogInformation("Fetching transactions for account...");
            var allTransactionsForAccount = await transactionRepository.GetAllByAccount(accountId);
            return Ok(allTransactionsForAccount);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "An error occured, while fetching transactions for account. Check if there is a account.");
            return BadRequest();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while fetching transactions for account.");
            return NotFound();
        }
    }
    
    
    [HttpPost("Add")]
    public async Task<ActionResult<Transaction>> AddNewTransaction([FromBody] TransactionBody transactionBody)
    {
        try
        {
            var transactionToAdd = new Transaction
            {
                Name = transactionBody.Name,
                AccountId = transactionBody.AccountId,
                Amount = transactionBody.Amount,
                CategoryId = transactionBody.CategoryId,
                Currency = transactionBody.Currency,
                Date = transactionBody.Date,
                Direction = transactionBody.Direction,
                IsRecurring = transactionBody.IsRecurring,
                RefreshDays = transactionBody.RefreshDays,
                Type = transactionBody.Type,
                SiblingTransactionId = transactionBody.SiblingTransactionId
            };

            await transactionRepository.AddNewTransaction(transactionToAdd);
            return Ok(transactionToAdd);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while adding the transaction to the database");
            return NotFound();
        }
        
    }

    [HttpDelete("Delete/{transactionId}")]
    public async Task<ActionResult> DeleteTransaction(int transactionId)
    {
        try
        {
            await transactionRepository.DeleteTransaction(transactionId);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            logger.LogError(e, "An error occured while deleting the transaction from the db. Please check transaction id.");
            return BadRequest();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while deleting the transaction from the db.");
            return NotFound();
        }
    }
}
