using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo;

[ApiController]
[Route("[controller]")]
public class ResourceController : Controller
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetResource()
    {
        var result = new Resource();
        return Ok(result);
    }
}