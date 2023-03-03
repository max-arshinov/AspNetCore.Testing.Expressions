using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpErrorCodesController : ControllerBase
{
    [HttpGet(nameof(Get400))]
    public ActionResult<string> Get400() => BadRequest();
    
    [HttpGet(nameof(Get404))]
    public ActionResult<string> Get404() => NotFound();
}