using AspNetCore.Testing.Expressions.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController: Controller
{
    [HttpGet]
    public string Get([FromServices] IService service) => service.GetString();
}