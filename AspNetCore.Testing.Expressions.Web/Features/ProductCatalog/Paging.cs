using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public class Paging
{
    [FromQuery]
    public int Page { get; init; } = 1;

    [FromQuery]
    public int Take { get; set; } = 50;
}