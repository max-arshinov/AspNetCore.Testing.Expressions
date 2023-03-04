using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static Dictionary<int, ProductDetails> _products = new()
    {
        { 1, new ProductDetails { Id = 1, Name = "1" } },
        { 2, new ProductDetails { Id = 2, Name = "2" } }
    };

    [HttpGet]
    public IEnumerable<ProductListItem> Get() => _products.Select(x => new ProductListItem()
    {
        Id = x.Value.Id,
        Name = x.Value.Name
    });

    [HttpGet("{id}")]
    public ProductDetails? Get([FromRoute]int id) => _products.ContainsKey(id) ? _products[id] : null;
}