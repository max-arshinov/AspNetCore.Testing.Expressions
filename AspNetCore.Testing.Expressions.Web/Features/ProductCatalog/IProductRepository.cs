namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public interface IProductRepository
{
    IEnumerable<ProductDetails> GetList(Paging paging);
    
    ProductDetails? GetById(int id);
}