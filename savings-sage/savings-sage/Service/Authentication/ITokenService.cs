using Microsoft.AspNetCore.Identity;

namespace savings_sage.Service.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}