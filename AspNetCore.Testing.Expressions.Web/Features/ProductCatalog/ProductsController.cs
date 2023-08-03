using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductListItem>> Get([FromQuery] Paging paging) =>
        _repository
        .GetList(paging)
        .Select(x => new ProductListItem()
        {
            Id = x.Id,
            Name = x.Name
        })
        .ToList();

    [HttpGet("{id}")]
    public ProductDetails? Get([FromRoute] int id) => _repository.GetById(id);

    [HttpPost]
    public string Post() => throw new NotImplementedException();
}