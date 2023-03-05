using AspNetCore.Testing.Expressions.Web.Features.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.Service;

[ApiController]
[Route("[controller]")]
public class ServiceController: Controller
{
    [HttpGet]
    public string Get([FromServices] IService service) => service.GetString();
}