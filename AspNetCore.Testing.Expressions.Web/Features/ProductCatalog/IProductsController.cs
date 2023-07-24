using Microsoft.AspNetCore.Mvc;
using Refit;

namespace AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;

public interface IProductsController
{
    [Get("/Products/{id}")]
    Task<ProductDetails?> GetAsync([FromRoute] int id);
}