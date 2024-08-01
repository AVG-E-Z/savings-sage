namespace SolarWatch.Contracts;

public record RegistrationResponse( string Email, 
    string UserName, bool Success);