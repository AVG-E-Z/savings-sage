// using Castle.Core.Configuration;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using MockQueryable.Moq;
// using Moq;
// using savings_sage.Controller;
// using savings_sage.Model;
// using savings_sage.Model.Accounts;
// using savings_sage.Service.Repositories;

// namespace SavingsSage_Tests;
//
// [TestFixture]
// public class AccountController_Test
// {
//     private Mock<ILogger<AccountController>> _loggerMock;
//     private Mock<IAccountRepository> _mockAccountRepository;
//     private Mock<UserManager<User>> _mockUserManager;
//     private AccountController _controller;
//
//     [SetUp]
//     public void Setup()
//     {
//         _loggerMock = new Mock<ILogger<AccountController>>();
//         _mockAccountRepository = new Mock<IAccountRepository>();
//         _mockUserManager = new Mock<UserManager<User>>();
//         _controller = new AccountController(
//             _loggerMock.Object,
//             _mockAccountRepository.Object,
//             _mockUserManager.Object);
//     }
//
//     #region GET
//     
//     [Test]
//     public async Task GetByUserIdReturnsNotFoundIfDatabaseIsEmpty()
//     {
//         //Arrange
//         string userName = "testUser";
//         _mockUserManager.Setup(x => x.Users)
//             .Returns(new List<User>().AsQueryable().BuildMock());
//         _mockAccountRepository.Setup(x => x.GetAllIdsByUser(It.IsAny<User>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.GetByUserId(userName);
//         
//         //Assert
//         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     }
//
//     [Test]
//     public async Task GetByIdReturnsNotFoundIfIdIsInvalid()
//     {
//         //Arrange
//         _mockAccountRepository.Setup(x => x.GetById(It.IsAny<int>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.GetById(100);
//         
//         //Assert
//         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     }
//     
//     [Test]
//     public async Task GetByIdReturnsOkIfIdIsValid()
//     {
//         //Arrange
//         var id = 100;
//
//         var expectedAccount = new Account
//         {
//             Id = 100,
//             Name = "name",
//             Currency = Currency.HUF,
//             OwnerId = "123",
//             Amount = 1000,
//             ParentAccountId = null,
//             GroupSharingOption = false,
//             CanGoMinus = false,
//             ExpirationDate = null,
//             Type = AccountType.Cash
//         };
//
//         _mockAccountRepository.Setup(x => x.GetById(id))
//             .ReturnsAsync(expectedAccount);
//         
//         //Act
//         var result = await _controller.GetById(id);
//         
//         //Assert
//         Assert.IsInstanceOf<OkObjectResult>(result.Result);
//         var okResult = result.Result as OkObjectResult;
//         Assert.NotNull(okResult);
//         var actualAccount = okResult.Value as BankAccount;
//         Assert.NotNull(actualAccount);
//         Assert.That(actualAccount.Id, Is.EqualTo(expectedAccount.Id));
//         Assert.That(actualAccount.Name, Is.EqualTo(expectedAccount.Name));
//         Assert.That(actualAccount.Currency, Is.EqualTo(expectedAccount.Currency));
//         Assert.That(actualAccount.OwnerId, Is.EqualTo(expectedAccount.OwnerId));
//         Assert.That(actualAccount.Amount, Is.EqualTo(expectedAccount.Amount));
//         Assert.That(actualAccount.ParentAccountId, Is.EqualTo(expectedAccount.ParentAccountId));
//         Assert.That(actualAccount.GroupSharingOption, Is.EqualTo(expectedAccount.GroupSharingOption));
//         Assert.That(actualAccount.CanGoMinus, Is.EqualTo(expectedAccount.CanGoMinus));
//         Assert.That(actualAccount.ExpirationDate, Is.EqualTo(expectedAccount.ExpirationDate));
//         Assert.That(actualAccount.Type, Is.EqualTo(expectedAccount.Type));
//     }
//     
//     [Test]
//     public async Task GetByOwnerIdReturnsNotFoundIfIdIsInvalid()
//     {
//         //Arrange
//         _mockAccountRepository.Setup(x => x.GetAllByOwner(It.IsAny<int>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.GetByOwnerId(100);
//         
//         //Assert
//         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     }
//     
//     [Test]
//     public async Task GetByOwnerIdReturnsOkIfIdIsValid()
//     {
//         //Arrange
//         var ownerId = 100;
//
//         var expectedAccount = new BankAccount
//         {
//             Id = 1,
//             Name = "name",
//             Currency = Currency.HUF,
//             OwnerId = 100,
//             Amount = 1000,
//             ParentAccountId = null,
//             GroupSharingOption = false,
//             CanGoMinus = false,
//             ExpirationDate = null,
//             Type = AccountType.Cash
//         };
//         var expectedAccounts = new List<BankAccount> { expectedAccount };
//
//         _mockAccountRepository.Setup(x => x.GetAllByOwner(ownerId))
//             .ReturnsAsync(expectedAccounts);
//         
//         //Act
//         var result = await _controller.GetByOwnerId(ownerId);
//         
//         //Assert
//         Assert.IsInstanceOf<OkObjectResult>(result.Result);
//         var okResult = result.Result as OkObjectResult;
//         Assert.NotNull(okResult);
//         var actualAccount = okResult.Value as IEnumerable<BankAccount>;
//         Assert.NotNull(actualAccount);
//         Assert.That(actualAccount.First().Id, Is.EqualTo(expectedAccount.Id));
//         Assert.That(actualAccount.First().Name, Is.EqualTo(expectedAccount.Name));
//         Assert.That(actualAccount.First().Currency, Is.EqualTo(expectedAccount.Currency));
//         Assert.That(actualAccount.First().OwnerId, Is.EqualTo(expectedAccount.OwnerId));
//         Assert.That(actualAccount.First().Amount, Is.EqualTo(expectedAccount.Amount));
//         Assert.That(actualAccount.First().ParentAccountId, Is.EqualTo(expectedAccount.ParentAccountId));
//         Assert.That(actualAccount.First().GroupSharingOption, Is.EqualTo(expectedAccount.GroupSharingOption));
//         Assert.That(actualAccount.First().CanGoMinus, Is.EqualTo(expectedAccount.CanGoMinus));
//         Assert.That(actualAccount.First().ExpirationDate, Is.EqualTo(expectedAccount.ExpirationDate));
//         Assert.That(actualAccount.First().Type, Is.EqualTo(expectedAccount.Type));
//     }
//     
//     [Test]
//     public async Task GetByOwnerByTypeIdReturnsNotFoundIfDataIsInvalid()
//     {
//         //Arrange
//         _mockAccountRepository.Setup(x => x.GetAllByOwnerByType(It.IsAny<int>(), It.IsAny<AccountType>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.GetByOwnerIdByType(100, AccountType.Cash);
//         
//         //Assert
//         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     }
//     
//     [Test]
//     public async Task GetByIdAllChildrenReturnsNotFoundIfDataIsInvalid()
//     {
//         //Arrange
//         _mockAccountRepository.Setup(x => x.GetAllSubAccounts(It.IsAny<int>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.GetByIdAllChildren(100);
//         
//         //Assert
//         Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
//     }
//
//     #endregion
//     
//     #region POST
//
//     
//     [Test]
//     public async Task Post_CreateNewAccountReturnsNotFoundIfDataIsInvalid()
//     {
//         //Arrange
//         var accountDTO = new BankAccountDataBody
//         {
//             Name = "name",
//             Amount = 1000,
//             Currency = Currency.HUF,
//             GroupSharingOption = false,
//             Type = AccountType.Cash
//         };
//         
//         _mockAccountRepository.Setup(x => x.AddAsync(It.IsAny<BankAccount>()))
//             .ThrowsAsync(new Exception());
//         
//         //Act
//         var result = await _controller.CreateNewAccount(accountDTO,1);
//         
//         //Assert
//         Assert.IsInstanceOf<ObjectResult>(result.Result);
//         
//         var objectResult = result.Result as ObjectResult;
//         Assert.IsNotNull(objectResult);
//         Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
//         
//         Assert.IsInstanceOf<ProblemDetails>(objectResult.Value); 
//         
//         var problemDetails = objectResult.Value as ProblemDetails;
//         Assert.IsNotNull(problemDetails);
//         Assert.That(problemDetails.Detail, Is.EqualTo("An error occurred while processing your request."));
//     }
//     [Test]
// public async Task CreateNewAccount_ReturnsCreatedAtAction_WhenDebitAccountCreatedSuccessfully()
// {
//     // Arrange
//     int ownerId = 1;
//     var accountDataBody = new BankAccountDataBody
//     {
//         Name = "Test Debit Account",
//         Currency = Currency.USD,
//         Amount = 100,
//         Type = AccountType.Debit
//     };
//     var createdAccount = new BankAccount
//     {
//         Id = 1,
//         Name = "Test Debit Account",
//         Currency = Currency.USD,
//         Amount = 100,
//         OwnerId = ownerId,
//         Type = AccountType.Debit
//     };
//
//     _mockAccountRepository.Setup(repo => repo.AddAsync(It.IsAny<BankAccount>())).ReturnsAsync(createdAccount);
//
//     // Act
//     var result = await _controller.CreateNewAccount(accountDataBody, ownerId);
//
//     // Assert
//     Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
//     var createdAtActionResult = result.Result as CreatedAtActionResult;
//     Assert.IsNotNull(createdAtActionResult);
//     Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(_controller.GetById)));
//     Assert.That(((BankAccount)createdAtActionResult.Value).Id, Is.EqualTo(createdAccount.Id));
// }
//
// [Test]
// public async Task CreateNewAccount_ReturnsCreatedAtAction_WhenCreditAccountCreatedSuccessfully()
// {
//     // Arrange
//     int ownerId = 1;
//     var accountDataBody = new BankAccountDataBody
//     {
//         Name = "Test Credit Account",
//         Currency = Currency.USD,
//         Amount = 200,
//         Type = AccountType.Credit
//     };
//     var createdAccount = new BankAccount
//     {
//         Id = 2,
//         Name = "Test Credit Account",
//         Currency = Currency.USD,
//         Amount = 200,
//         OwnerId = ownerId,
//         Type = AccountType.Credit
//     };
//
//     _mockAccountRepository.Setup(repo => repo.AddAsync(It.IsAny<BankAccount>())).ReturnsAsync(createdAccount);
//
//     // Act
//     var result = await _controller.CreateNewAccount(accountDataBody, ownerId);
//
//     // Assert
//     Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
//     var createdAtActionResult = result.Result as CreatedAtActionResult;
//     Assert.IsNotNull(createdAtActionResult);
//     Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(_controller.GetById)));
//     Assert.That(((BankAccount)createdAtActionResult.Value).Id, Is.EqualTo(createdAccount.Id));
// }
//
// [Test]
// public async Task CreateNewAccount_ReturnsCreatedAtAction_WhenLoanAccountCreatedSuccessfully()
// {
//     // Arrange
//     int ownerId = 1;
//     var accountDataBody = new BankAccountDataBody
//     {
//         Name = "Test Loan Account",
//         Currency = Currency.USD,
//         Amount = 300,
//         AmountInterest = 50,
//         AmountCapital = 250,
//         Type = AccountType.Loan
//     };
//     var createdAccount = new BankAccount
//     {
//         Id = 3,
//         Name = "Test Loan Account",
//         Currency = Currency.USD,
//         Amount = 300,
//         OwnerId = ownerId,
//         Type = AccountType.Loan
//     };
//
//     _mockAccountRepository.Setup(repo => repo.AddAsync(It.IsAny<BankAccount>())).ReturnsAsync(createdAccount);
//
//     // Act
//     var result = await _controller.CreateNewAccount(accountDataBody, ownerId);
//
//     // Assert
//     Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
//     var createdAtActionResult = result.Result as CreatedAtActionResult;
//     Assert.IsNotNull(createdAtActionResult);
//     Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(_controller.GetById)));
//     Assert.That(((BankAccount)createdAtActionResult.Value).Id, Is.EqualTo(createdAccount.Id));
// }
//     
//     #endregion
//     
//     #region DELETE
//
//     
//     [Test]
//     public async Task DeleteAccountAndSubAccounts_ReturnsNotFound_WhenAccountDoesNotExist()
//     {
//         // Arrange
//         int userId = 1;
//         int accountId = 1;
//         _mockAccountRepository.Setup(repo => repo.GetById(accountId)).ReturnsAsync((BankAccount)null);
//
//         // Act
//         var result = await _controller.DeleteAccountAndSubAccounts(userId, accountId);
//
//         // Assert
//         Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
//         var notFoundResult = result.Result as NotFoundObjectResult;
//         Assert.That(notFoundResult.Value, Is.EqualTo($"Account with {accountId} not found."));
//     }
//     
//     
//     [Test]
//     public async Task DeleteAccountAndSubAccounts_ReturnsForbid_WhenUserIsNotOwner()
//     {
//         // Arrange
//         int userId = 1;
//         int accountId = 1;
//         var account = new BankAccount { Id = accountId, OwnerId = 2 };
//         _mockAccountRepository.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);
//
//         // Act
//         var result = await _controller.DeleteAccountAndSubAccounts(userId, accountId);
//
//         // Assert
//         Assert.IsInstanceOf<ForbidResult>(result.Result);
//     }
//     
//     [Test]
//     public async Task DeleteAccountAndSubAccounts_ReturnsNotFound_WhenExceptionThrown()
//     {
//         // Arrange
//         int userId = 1;
//         int accountId = 1;
//         _mockAccountRepository.Setup(repo => repo.GetById(accountId)).ThrowsAsync(new Exception());
//
//         // Act
//         var result = await _controller.DeleteAccountAndSubAccounts(userId, accountId);
//
//         // Assert
//         Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
//         var notFoundResult = result.Result as NotFoundObjectResult;
//         Assert.That(notFoundResult.Value, Is.EqualTo("Error deleting account"));
//     }
//     
//     #endregion
//     
//     #region PUT
//
//     [Test]
//     public async Task UpdateAccount_ReturnsNotFound_WhenAccountDoesNotExist()
//     {
//         // Arrange
//         int userId = 1;
//         int accountId = 1;
//         var accountDataBody = new BankAccountDataBody { /* Populate properties */ };
//         _mockAccountRepository.Setup(repo => repo.GetById(accountId)).ReturnsAsync((BankAccount)null);
//
//         // Act
//         var result = await _controller.UpdateAccount(accountDataBody, userId, accountId);
//
//         // Assert
//         Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
//         var notFoundResult = result.Result as NotFoundObjectResult;
//         Assert.That(notFoundResult.Value, Is.EqualTo($"Account with {accountId} not found."));
//     }
//
//     [Test]
//     public async Task UpdateAccount_ReturnsForbid_WhenUserIsNotOwner()
//     {
//         // Arrange
//         int userId = 1;
//         int accountId = 1;
//         var accountDataBody = new BankAccountDataBody { /* Populate properties */ };
//         var account = new BankAccount { Id = accountId, OwnerId = 2 };
//         _mockAccountRepository.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);
//
//         // Act
//         var result = await _controller.UpdateAccount(accountDataBody, userId, accountId);
//
//         // Assert
//         Assert.IsInstanceOf<ForbidResult>(result.Result);
//     }
//     #endregion
//}