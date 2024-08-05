using System.Text;
using Newtonsoft.Json;
using savings_sage.Contracts;
using SavingsSage_IntegrationTESTS.Factories;
using Xunit.Abstractions;

namespace SavingsSage_IntegrationTESTS.Tests;

[Collection("IntegrationTests")]
public class SavingsSage_AuthControllerTests
{
    private readonly SavingsSageWebApp_FACTORY _app;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper output;

    public SavingsSage_AuthControllerTests(ITestOutputHelper output)
    {
        _app = new SavingsSageWebApp_FACTORY();
        _client = _app.CreateClient();
        this.output = output;
    }


    [Fact]
    public async Task TestGetCurrentEndPoint()
    {
        var loginRequest = new AuthRequest("admin@admin.com", "Password123!");
        var loginResponse = await _client.PostAsync("api/Auth/login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        var userToken = authResponse.Token;

        // Assert
        Assert.NotNull(authResponse.Token);
        Assert.Equal("admin@admin.com", authResponse.Email);
        Assert.Equal("admin", authResponse.UserName);
        output.WriteLine(userToken);
    }

}