using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using savings_sage.Controller;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using savings_sage.Service.Repositories;

namespace SavingsSage_Tests;

[TestFixture]
public class BankAccountController_Test
{
    private Mock<ILogger<BankAccountController>> _loggerMock;
    private Mock<IBankAccountRepository> _mockBankAccountRepository;
    private BankAccountController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<BankAccountController>>();
        _mockBankAccountRepository = new Mock<IBankAccountRepository>();
        _controller = new BankAccountController(
            _loggerMock.Object,
            _mockBankAccountRepository.Object);
    }

    [Test]
    public async Task GetAllAccountsReturnsNotFoundIfDatabaseIsEmpty()
    {
        //Arrange
        _mockBankAccountRepository.Setup(x => x.GetAll())
            .ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.GetAllAccounts();
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public async Task GetByIdReturnsNotFoundIfIdIsInvalid()
    {
        //Arrange
        _mockBankAccountRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.GetById(100);
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetByIdReturnsOkIfIdIsValid()
    {
        //Arrange
        var id = 100;

        var expectedAccount = new BankAccount
        {
            Id = 100,
            Name = "name",
            Currency = Currency.HUF,
            OwnerId = 1,
            Amount = 1000,
            ParentAccountId = null,
            GroupSharingOption = false,
            CanGoMinus = false,
            ExpirationDate = null,
            Type = AccountType.Cash
        };

        _mockBankAccountRepository.Setup(x => x.GetById(id))
            .ReturnsAsync(expectedAccount);
        
        //Act
        var result = await _controller.GetById(id);
        
        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var actualAccount = okResult.Value as BankAccount;
        Assert.NotNull(actualAccount);
        Assert.That(actualAccount.Id, Is.EqualTo(expectedAccount.Id));
        Assert.That(actualAccount.Name, Is.EqualTo(expectedAccount.Name));
        Assert.That(actualAccount.Currency, Is.EqualTo(expectedAccount.Currency));
        Assert.That(actualAccount.OwnerId, Is.EqualTo(expectedAccount.OwnerId));
        Assert.That(actualAccount.Amount, Is.EqualTo(expectedAccount.Amount));
        Assert.That(actualAccount.ParentAccountId, Is.EqualTo(expectedAccount.ParentAccountId));
        Assert.That(actualAccount.GroupSharingOption, Is.EqualTo(expectedAccount.GroupSharingOption));
        Assert.That(actualAccount.CanGoMinus, Is.EqualTo(expectedAccount.CanGoMinus));
        Assert.That(actualAccount.ExpirationDate, Is.EqualTo(expectedAccount.ExpirationDate));
        Assert.That(actualAccount.Type, Is.EqualTo(expectedAccount.Type));
    }
    
    [Test]
    public async Task GetByOwnerIdReturnsNotFoundIfIdIsInvalid()
    {
        //Arrange
        _mockBankAccountRepository.Setup(x => x.GetAllByOwner(It.IsAny<int>()))
            .ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.GetByOwnerId(100);
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetByOwnerIdReturnsOkIfIdIsValid()
    {
        //Arrange
        var ownerId = 100;

        var expectedAccount = new BankAccount
        {
            Id = 1,
            Name = "name",
            Currency = Currency.HUF,
            OwnerId = 100,
            Amount = 1000,
            ParentAccountId = null,
            GroupSharingOption = false,
            CanGoMinus = false,
            ExpirationDate = null,
            Type = AccountType.Cash
        };
        var expectedAccounts = new List<BankAccount> { expectedAccount };

        _mockBankAccountRepository.Setup(x => x.GetAllByOwner(ownerId))
            .ReturnsAsync(expectedAccounts);
        
        //Act
        var result = await _controller.GetByOwnerId(ownerId);
        
        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var actualAccount = okResult.Value as IEnumerable<BankAccount>;
        Assert.NotNull(actualAccount);
        Assert.That(actualAccount.First().Id, Is.EqualTo(expectedAccount.Id));
        Assert.That(actualAccount.First().Name, Is.EqualTo(expectedAccount.Name));
        Assert.That(actualAccount.First().Currency, Is.EqualTo(expectedAccount.Currency));
        Assert.That(actualAccount.First().OwnerId, Is.EqualTo(expectedAccount.OwnerId));
        Assert.That(actualAccount.First().Amount, Is.EqualTo(expectedAccount.Amount));
        Assert.That(actualAccount.First().ParentAccountId, Is.EqualTo(expectedAccount.ParentAccountId));
        Assert.That(actualAccount.First().GroupSharingOption, Is.EqualTo(expectedAccount.GroupSharingOption));
        Assert.That(actualAccount.First().CanGoMinus, Is.EqualTo(expectedAccount.CanGoMinus));
        Assert.That(actualAccount.First().ExpirationDate, Is.EqualTo(expectedAccount.ExpirationDate));
        Assert.That(actualAccount.First().Type, Is.EqualTo(expectedAccount.Type));
    }
    
    [Test]
    public async Task GetByOwnerByTypeIdReturnsNotFoundIfDataIsInvalid()
    {
        //Arrange
        _mockBankAccountRepository.Setup(x => x.GetAllByOwnerByType(It.IsAny<int>(), It.IsAny<AccountType>()))
            .ThrowsAsync(new Exception());
        
        //Act
        var result = await _controller.GetByOwnerIdByType(100, AccountType.Cash);
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
}