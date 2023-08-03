using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.ErrorCodes;

[ApiController]
[Route("[controller]")]
public class ErrorCodeController: Controller
{
    [HttpGet("400")]
    public ActionResult<string> Get400() => new ObjectResult("string") { StatusCode = 400 };

    [HttpGet("500")]
    public ActionResult<string> Get500() => new ObjectResult("string") { StatusCode = 500 };
}