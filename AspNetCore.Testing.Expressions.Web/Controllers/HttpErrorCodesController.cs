using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpErrorCodesController : Controller
{
    public ActionResult<string> Get400() => BadRequest();
}