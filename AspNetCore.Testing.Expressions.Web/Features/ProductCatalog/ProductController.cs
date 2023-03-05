using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public IEnumerable<ProductListItem> Get() => _repository.GetAll().Select(x => new ProductListItem()
    {
        Id = x.Id,
        Name = x.Name
    });

    [HttpGet("{id}")]
    public ProductDetails? Get([FromRoute] int id) => _repository.GetById(id);
}