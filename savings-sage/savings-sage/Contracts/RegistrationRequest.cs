using System.ComponentModel.DataAnnotations;

namespace savings_sage.Contracts;

public record RegistrationRequest(
    [Required] string Email,
    [Required] string Username,
    [Required] string Password);