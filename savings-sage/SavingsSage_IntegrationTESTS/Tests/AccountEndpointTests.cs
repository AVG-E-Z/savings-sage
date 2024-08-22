using System.Text;
using Newtonsoft.Json;
using savings_sage.Model;
using savings_sage.Model.Accounts;
using SavingsSage_IntegrationTESTS.Factories;
using Xunit.Abstractions;

namespace SavingsSage_IntegrationTESTS.Tests;

public class AccountResponse
{
    public bool Ok { get; set; }
    public Account Account { get; set; }
}

public class AccountEndpointTests
{
    private readonly SavingsSageWebApp_FACTORY _app;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper output;
    private readonly LoginTestService _loginTestService;

    public AccountEndpointTests(ITestOutputHelper output)
    {
        _app = new SavingsSageWebApp_FACTORY();
        _loginTestService = new LoginTestService(output);
        _client = _app.CreateClient();
        this.output = output;
    }
    
    private readonly AccountDataBody accountData1 = new AccountDataBody
        {
            Name = "teszt 1",
            Amount = 1000,
            Currency = Currency.HUF,
            Type = "Cash"
        };
    
    [Fact]
    public async Task CreateNewAccount()
    {
        var authResponse = await _loginTestService.LoginUser();
        var token = authResponse.Token;
        var userName = authResponse.UserName;
    
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
        var content = new StringContent(JsonConvert.SerializeObject(accountData1), Encoding.UTF8, "application/json");
        
        var createResponse = await _client.PostAsync($"api/Account/u/Add", content);
    
        if (!createResponse.IsSuccessStatusCode)
        {
            var errorContent = await createResponse.Content.ReadAsStringAsync();
            output.WriteLine($"Failed to create account. Status Code: {createResponse.StatusCode}, Response: {errorContent}");
            return; 
        }
    
        var response = JsonConvert.DeserializeObject<AccountResponse>(await createResponse.Content.ReadAsStringAsync());
        
        if (response == null)
        {
            output.WriteLine("Response is null.");
            return;
        }
    
        output.WriteLine(response.Account.OwnerId);
        Assert.Equal(userName, response.Account.Owner.UserName);
        Assert.Equal(accountData1.Name, response.Account.Name);
    }
    
    [Fact]
    public async Task CreateNewSubAccount()
    {
        var authResponse = await _loginTestService.LoginUser();
        var token = authResponse.Token;
        var userName = authResponse.UserName;
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
        var content = new StringContent(JsonConvert.SerializeObject(accountData1), Encoding.UTF8, "application/json");
        
        var createResponse = await _client.PostAsync($"api/Account/u/Add", content);
    
        if (!createResponse.IsSuccessStatusCode)
        {
            var errorContent = await createResponse.Content.ReadAsStringAsync();
            output.WriteLine($"Failed to create account. Status Code: {createResponse.StatusCode}, Response: {errorContent}");
            return;
        }
        
        var response = JsonConvert.DeserializeObject<AccountResponse>(await createResponse.Content.ReadAsStringAsync());
        
        output.WriteLine("first Acc. Id: "+response.Account.Id);

        var accountData2 = new AccountDataBody()
        {
            Name = "teszt 2",
            Amount = 2000,
            Currency = Currency.HUF,
            Type = "Cash",
            ParentAccountId = response.Account.Id
        };
        
        var contentSubAcc = new StringContent(JsonConvert.SerializeObject(accountData2), Encoding.UTF8, "application/json");
        
        var createSubResponse = await _client.PostAsync($"api/Account/u/Add", contentSubAcc);
    
        if (!createSubResponse.IsSuccessStatusCode)
        {
            var errorContent = await createSubResponse.Content.ReadAsStringAsync();
            output.WriteLine($"Failed to create account. Status Code: {createSubResponse.StatusCode}, Response: {errorContent}");
            return; 
        }
    
        var responseSubAcc = JsonConvert.DeserializeObject<AccountResponse>(await createSubResponse.Content.ReadAsStringAsync());
    
        Assert.Equal(userName, responseSubAcc.Account.Owner.UserName);
        Assert.Equal(2, responseSubAcc.Account.SubAccounts.First().Id);
        Assert.Equal(accountData2.Name, responseSubAcc.Account.SubAccounts.First().Name);
    }
}