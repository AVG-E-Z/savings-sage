using Microsoft.AspNetCore.Mvc;
using SolarWatch.Contracts;
using SolarWatch.Services.Authentication;
using SolarWatchMVC.ViewModels;

namespace SolarWatch.Controllers;

public class AuthController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IAuthService _authenticationService;

    public AuthController(IAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.RegisterAsync(model.Email, model.Username, model.Password, "User");

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return RedirectToAction("Login");
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        var result = await _authenticationService.LoginAsync(model.Email, model.Password);
    
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }
    
        HttpContext.Session.SetString("token", result.Token);
        
        return RedirectToAction("SolarWatch", "SolarWatch");
    }
}