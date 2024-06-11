using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SavingsSage;

[ApiController]
[Route("[controller]")]
public class DummyController : Controller
{
    [HttpGet]
    public IActionResult SimpleGetRequest()
    {
        var response = new { hi = "Hello!" };
        return new JsonResult(response);
    }
}