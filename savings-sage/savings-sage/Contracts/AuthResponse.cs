namespace savings_sage.Contracts;

public record AuthResponse(bool success, string Email, string UserName, string Token);