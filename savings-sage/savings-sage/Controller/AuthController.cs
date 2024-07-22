using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using savings_sage.Contracts;
using savings_sage.Service.Authentication;

namespace SolarWatch.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authenticationService, IConfiguration configuration)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password,
            _configuration["Roles:User"]);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages) ModelState.AddModelError(error.Key, error.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authenticationService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };
        
        Response.Cookies.Append("jwt", result.Token, cookieOptions);

        return Ok(new AuthResponse(true,result.Email, result.UserName, result.Token));
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new { message = "Logged out" });
    }
    
    [HttpGet("me")]
    //[Authorize(Policy = "RequiredUserOrAdminRole")]
    public IActionResult Me()
    {
        var username = User.Identity.Name;
        return Ok(new { username });
    }
}