using ProductsCrudApi.Products.Model;
using ProductsCrudApi.Products.Repository;
using ProductsCrudApi.Products.Repository.Interfaces;
using ProductsCrudApi.Products.Service.Interfaces;
using ProductsCrudApi.System.Constants;
using ProductsCrudApi.System.Exceptions;

namespace ProductsCrudApi.Products.Service;

public class ProductQueryService : IProductQuerryService
{
    private IProductRepository _repository;

    public ProductQueryService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        IEnumerable<Product> products = await _repository.GetAllAsync();

        if (products.Count() == 0)
        {
            throw new ItemsDoNotExist(Constants.NO_PRODUCTS_EXIST);
        }

        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsWithCategory(string category)
    {
        IEnumerable<Product> products = (await _repository.GetAllAsync())
            .Where(product => product.Category.Equals(category));

        if (products.Count() == 0)
        {
            throw new ItemsDoNotExist(Constants.NO_PRODUCTS_EXIST);
        }

        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsWithNoCategory()
    {
        IEnumerable<Product> products = (await _repository.GetAllAsync())
            .Where(product => product.Category == null!);

        if (products.Count() == 0)
        {
            throw new ItemsDoNotExist(Constants.NO_PRODUCTS_EXIST);
        }

        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsInPriceRange(double min, double max)
    {
        IEnumerable<Product> products = (await _repository.GetAllAsync())
            .Where(product => product.Price >= min && product.Price <= max);

        if (products.Count() == 0)
        {
            throw new ItemsDoNotExist(Constants.NO_PRODUCTS_EXIST);
        }

        return products;
    }

    public async Task<Product> GetProductById(int id)
    {
        Product product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new ItemDoesNotExist(Constants.PRODUCT_DOES_NOT_EXIST);
        }

        return product;
    }
}
