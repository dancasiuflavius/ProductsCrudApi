using ProductsCrudApi.Products.Model;

namespace ProductsCrudApi.Products.Service.Interfaces
{
    public interface IProductQuerryService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetProductsWithCategory(string category);
        Task<IEnumerable<Product>> GetProductsWithNoCategory();
        Task<IEnumerable<Product>> GetProductsInPriceRange(double min, double max);
        Task<Product> GetProductById(int id);
    }
}
