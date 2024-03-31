using ProductsCrudApi.Products.Dto;
using ProductsCrudApi.Products.Model;


namespace ProductsCrudApi.Products.Service.Interfaces;


public interface IProductComandService
{
    Task<Product> CreateProduct(CreateProductRequest productRequest);

    Task<Product> UpdateProduct(int id,UpdateProductRequest productRequest);

    Task<Product> DeleteProduct(int id);
}
