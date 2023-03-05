namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public class ProductRepository : IProductRepository
{
    private static Dictionary<int, ProductDetails> _products = new()
    {
        { 1, new ProductDetails { Id = 1, Name = "1" } },
        { 2, new ProductDetails { Id = 2, Name = "2" } }
    };

    public IEnumerable<ProductDetails> GetAll() => _products.Values;
    
    public ProductDetails? GetById(int id) => _products.ContainsKey(id) ? _products[id] : null;
}