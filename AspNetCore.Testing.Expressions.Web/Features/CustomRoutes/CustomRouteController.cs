using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.CustomRoutes;

[ApiController]
[Route("custom-controller-route")]
public class CustomRouteController: Controller
{
    [HttpGet]
    [Route("custom/action-route")]
    public ActionResult<string> CustomRoute() => "custom route";
}