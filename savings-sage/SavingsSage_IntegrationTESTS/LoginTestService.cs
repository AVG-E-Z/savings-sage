using System.Text;
using Newtonsoft.Json;
using savings_sage.Contracts;
using SavingsSage_IntegrationTESTS.Factories;
using Xunit.Abstractions;

namespace SavingsSage_IntegrationTESTS;

public class LoginTestService
{
    private readonly SavingsSageWebApp_FACTORY _app;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper output;

    public LoginTestService(ITestOutputHelper output)
    {
        _app = new SavingsSageWebApp_FACTORY();
        _client = _app.CreateClient();
        this.output = output;
    }

    public async Task<AuthResponse> LoginAdmin()
    {
        var loginRequest = new AuthRequest("admin@admin.com", "Password123!");
        var loginResponse = await _client.PostAsync("api/Auth/login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        return authResponse;
    }
    
    public async Task<AuthResponse> LoginUser()
    {
        var loginRequest = new AuthRequest("teszt@teszt.com", "asd123");
        var loginResponse = await _client.PostAsync("api/Auth/login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        return authResponse;
    }
 
}