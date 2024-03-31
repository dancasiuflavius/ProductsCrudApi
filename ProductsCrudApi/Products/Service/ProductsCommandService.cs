using ProductsCrudApi.Products.Dto;
using ProductsCrudApi.Products.Model;
using ProductsCrudApi.Products.Repository;
using ProductsCrudApi.Products.Repository.Interfaces;
using ProductsCrudApi.Products.Service.Interfaces;
using ProductsCrudApi.System.Constants;
using ProductsCrudApi.System.Exceptions;

namespace ProductsCrudApi.Products.Service
{
    public class ProductsCommandService : IProductComandService
    {
        private IProductRepository _repository;

        public ProductsCommandService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> CreateProduct(CreateProductRequest productRequest)
        {
            if(productRequest.Price < 0)
            {
                throw new InvalidPrice(Constants.INVALID_PRICE);
            }

            Product product = await _repository.GetByNameAsync(productRequest.Name);

            if(product !=null)
            {
                throw new ItemAlreadyExists(Constants.PRODUCT_ALREADY_EXISTS);
            }

            product = await _repository.CreateAsync(productRequest);
            return product;
        }
        public async Task<Product> UpdateProduct(int id,UpdateProductRequest productRequest)
        {
            if (productRequest.Price < 0)
            {
                throw new InvalidPrice(Constants.INVALID_PRICE);
            }

            Product product = await _repository.GetByIdAsync(productRequest.Id);
            if (product == null)
            {
                throw new ItemDoesNotExist(Constants.PRODUCT_DOES_NOT_EXIST);
            }
            product = await _repository.UpdateAsync(id,productRequest);
            return product;
        }
        public async Task<Product> DeleteProduct(int id)
        {
            Product product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ItemDoesNotExist(Constants.PRODUCT_DOES_NOT_EXIST);
            }

            await _repository.DeleteAsync(id);
            return product;
        }
    }
}
