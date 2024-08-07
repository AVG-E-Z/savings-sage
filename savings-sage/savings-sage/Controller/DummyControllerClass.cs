using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace savings_sage.Controller;

[ApiController]
[Route("api/[controller]")]
public class DummyController : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpGet]
    //[Authorize(Policy = "RequiredUserOrAdminRole")]
    //[Authorize(Policy = "RequiredUserRole")]
    //[Authorize(Policy = "RequiredAdminRole")]
    public IActionResult SimpleGetRequest()
    {
        try
        {
            var response = new { hi = "Hello!" };
            return new JsonResult(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e + " backend error");
            throw;
        }
    }
}