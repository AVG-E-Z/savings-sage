using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using savings_sage.Contracts;
using savings_sage.Model;
using savings_sage.Service.Authentication;
using savings_sage.Service.Repositories;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly ICategoryRepository _categoryRepository;

    public AuthController(IAuthService authenticationService, IConfiguration configuration, UserManager<User> userManager, ICategoryRepository categoryRepository)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
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
        
        var userName = result.UserName;
        var user = await  _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            return BadRequest("User not found after registration");
        }

        var userId = user.Id;
         await _categoryRepository.CreateDefaultCategoriesAsync(userId);
        
        
        return CreatedAtAction(nameof(Register), new RegistrationResponse(true,result.Email, result.UserName));
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
            Expires = DateTime.UtcNow.AddHours(1),
        };
        
        Response.Cookies.Append("access_token", result.Token, cookieOptions);

        return Ok(new AuthResponse(true,result.Email, result.UserName, result.Token));
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new { message = "Logged out" });
    }
    
    [HttpGet("me")]
    [Authorize(Policy = "RequiredUserOrAdminRole")]
    public IActionResult Me()
    {
        var username = User.Identity.Name;
        return Ok(new { username });
    }

}