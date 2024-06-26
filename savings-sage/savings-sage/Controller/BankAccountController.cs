using Microsoft.AspNetCore.Mvc;
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
    
}