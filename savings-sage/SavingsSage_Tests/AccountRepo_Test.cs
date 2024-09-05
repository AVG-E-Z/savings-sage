 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Configuration;
 using Moq;
 using savings_sage.Context;
 using savings_sage.Model;
 using savings_sage.Model.Accounts;
 using savings_sage.Service.Repositories;

 namespace SavingsSage_Tests;

 [TestFixture]
 public class AccountRepository_Tests
 {
     private Mock<IConfiguration> _config;
     private AccountRepository _accountRepository;

     public AccountRepository_Tests()
     {
         var options = new DbContextOptionsBuilder<SavingsSageContext>()
             .UseInMemoryDatabase(databaseName: "SavingsSageTestDB")
             .Options;
         _config = new Mock<IConfiguration>();

         var context = new SavingsSageContext(options, _config.Object);

         context.Accounts.AddRange(new List<Account>
         {
             new Account {Id = 1, Name = "Account 1", OwnerId = "one", Type = AccountType.Savings, Amount = 1000, Currency = Currency.HUF, CanGoMinus = false, GroupSharingOption = false },
             new Account {Id = 2, Name = "Account 2", OwnerId = "one", Type = AccountType.Cash, Amount = 1000, Currency = Currency.HUF, CanGoMinus = false, GroupSharingOption = false, ParentAccountId = 1},
             new Account {Id = 3, Name = "Account 3", OwnerId = "two", Type = AccountType.Savings, Amount = 1000, Currency = Currency.HUF, CanGoMinus = false, GroupSharingOption = false}
         });
         context.SaveChanges();

         _accountRepository = new AccountRepository(context);
     }

     private static readonly object[] TestCases1 =
     {
         new object[] {1, "Account 1"},
         new object[] {2, "Account 2"},
         new object[] {3, "Account 3"},
         new object[] {4, null},

     };

     [TestCaseSource(nameof(TestCases1))] 
     public async Task GetAccountById_ReturnsCorrectAccount(int accountId, string expectedName)
     {
         var result = await _accountRepository.GetByIdAsync(accountId);

         if (expectedName != null)
         {
             Assert.NotNull(result);
             Assert.That(expectedName, Is.EqualTo(result.Name));
         }
         else
         {
             Assert.Null(result);
         }
     }
     
     
     [Test]
     public async Task GetAllAccounts_ReturnsNumberOfAccounts()
     {
         var result = await _accountRepository.GetAll();

         Assert.That(result.Count, Is.EqualTo(3));
     }

     [Test]
     public async Task GetAccountByOwnerId_ReturnsCorrectAccount()
     {
         var result = await _accountRepository.GetAllByOwner("one");
         
         Assert.That(result.Count, Is.EqualTo(2));
     }
     
     [Test]
     public async Task GetAccountByOwnerIdByType_ReturnsCorrectAccount()
     {
         var result = await _accountRepository.GetAllByOwnerByType("one", AccountType.Cash);
         
         Assert.That(result.Count, Is.EqualTo(1));
     }
     
     [Test]
     public async Task GetAllSubAccounts_ReturnsCorrectAccount()
     {
         var result = await _accountRepository.GetAllSubAccounts(1);
         
         Assert.That(result.Count, Is.EqualTo(1));
         Assert.That(result.First().Name, Is.EqualTo("Account 2"));
     }
}