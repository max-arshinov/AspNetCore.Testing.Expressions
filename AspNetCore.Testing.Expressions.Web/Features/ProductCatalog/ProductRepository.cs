namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public class ProductRepository : IProductRepository
{
    private static Dictionary<int, ProductDetails> _products = new()
    {
        { 1, new ProductDetails { Id = 1, Name = "1" } },
        { 2, new ProductDetails { Id = 2, Name = "2" } },
        { 3, new ProductDetails { Id = 3, Name = "3" } },
        { 4, new ProductDetails { Id = 4, Name = "4" } },
        { 5, new ProductDetails { Id = 5, Name = "5" } },
    };

    public IEnumerable<ProductDetails> GetList(Paging paging) => _products
        .Values
        .Skip((paging.Page - 1) * paging.Take)
        .Take(paging.Take)
        .ToList();
    
    public ProductDetails? GetById(int id) => _products.ContainsKey(id) ? _products[id] : null;
}