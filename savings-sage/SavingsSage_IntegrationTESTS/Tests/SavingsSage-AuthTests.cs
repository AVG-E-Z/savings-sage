using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    private readonly LoginTestService _loginTestService;

    public SavingsSage_AuthControllerTests(ITestOutputHelper output)
    {
        _app = new SavingsSageWebApp_FACTORY();
        _loginTestService = new LoginTestService(output);
        _client = _app.CreateClient();
        this.output = output;
    }


    [Fact]
    public async Task TestGetCurrentEndPoint()
    {
        var loginRequest = new AuthRequest("admin@admin.com", "Password123!");
        var loginRequestJson = JsonSerializer.Serialize(loginRequest);
       
        var loginResponse = await _client.PostAsync("api/Auth/login",
            new StringContent(loginRequestJson, Encoding.UTF8, "application/json"));
        
        var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthResponse>(loginResponseContent);

        // Assert
        Assert.NotNull(authResponse.Token);
        Assert.Equal("admin@admin.com", authResponse.Email);
        Assert.Equal("admin", authResponse.UserName);
        output.WriteLine(authResponse.Token);
    }

    [Fact]
    public async Task TestMeEndpoint()
    {
        var authResponse = await _loginTestService.LoginUser();
        var token = authResponse.Token;
        
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new HttpRequestMessage(HttpMethod.Get, "api/Auth/me");
        var response = await _client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}