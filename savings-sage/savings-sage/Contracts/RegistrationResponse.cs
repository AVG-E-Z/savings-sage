namespace savings_sage.Contracts;

public record RegistrationResponse(bool Success,
    string Email,
    string UserName);