using SavingsSage_IntegrationTESTS.Factories;
using Xunit.Abstractions;

namespace SavingsSage_IntegrationTESTS.Tests;

public class TransactionEndpointTests
{
    private readonly SavingsSageWebApp_FACTORY _app;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper output;
    private readonly LoginTestService _loginTestService;

    public TransactionEndpointTests(ITestOutputHelper output)
    {
        _app = new SavingsSageWebApp_FACTORY();
        _loginTestService = new LoginTestService(output);
        _client = _app.CreateClient();
        this.output = output;
    }

    // [Fact]
    // public async Task AddNewTransaction()
    // {
    //     
    // }
}