using Microsoft.AspNetCore.Mvc;

namespace savings_sage.Controller;

[ApiController]
[Route("[controller]")]
public class DummyController : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpGet]
    public IActionResult SimpleGetRequest()
    {
        var response = new { hi = "Hello!" };
        return new JsonResult(response);
    }
}