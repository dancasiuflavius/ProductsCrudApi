using ProductsCrudApi.Products.Dto;
using ProductsCrudApi.Products.Model;

namespace ProductsCrudApi.Products.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByNameAsync(string name);

        Task<IEnumerable<Double>> GetAllAsyncPrice();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(CreateProductRequest productRequest);
        Task<Product> UpdateAsync(int id, UpdateProductRequest productRequest);
        Task<Product> DeleteAsync(int id);
    }
}
