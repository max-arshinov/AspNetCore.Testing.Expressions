using System.Diagnostics.CodeAnalysis;
using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using AspNetCore.Testing.Expressions.Web.Features.Service.Services;
using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ProductControllerWafTests :
    ProductControllerTestsBase<MoqHttpClientFactory<ProductsController>>
{
    private readonly string _stringMock = "__STRING__";

    public ProductControllerWafTests(MoqHttpClientFactory<ProductsController> http) : base(http)
    {
        HttpClientFactory.ConfigureMocks(m =>
        {
            m.Mock<IProductRepository>()   
                .Setup(x => x.GetList(It.IsAny<Paging>()))
                .Returns(() => new []
                {
                    new ProductDetails()
                    {
                        Id = 1050
                    },
                    new ProductDetails()
                    {
                        Id = 2060
                    },
                    new ProductDetails()
                    {
                        Id = 3070
                    }                        
                });

            m.Mock<IService>()   
                .Setup(x => x.GetString())
                .Returns(() => _stringMock);            
        });
        
        HttpClientFactory.ConfigureMocks(nameof(A), m =>
        {
            m.Mock<IProductRepository>()   
                .Setup(x => x.GetList(It.IsAny<Paging>()))
                .Returns(() => new []
                {
                    new ProductDetails()
                    {
                        Id = 100500
                    },
                    new ProductDetails()
                    {
                        Id = 200600
                    }
                });
        });

        HttpClientFactory.ConfigureMocks(nameof(LoadMany_GetById_CanFetchAllProductsFromGetAll), m =>
        {
            var productRepo = m.Mock<IProductRepository>();
            productRepo
                .Setup(x => x.GetList(It.IsAny<Paging>()))
                .Returns(() => new ProductRepository().GetList(new Paging()));

            productRepo
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => new ProductRepository().GetById(id));
        });
    }
}