namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public interface IProductRepository
{
    IEnumerable<ProductDetails> GetAll();
    ProductDetails? GetById(int id);
}