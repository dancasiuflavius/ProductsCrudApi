using ProductsCrudApi.Products.Controller;
using ProductsCrudApi.Products.Controller.Interfaces;
using ProductsCrudApi.Products.Dto;
using ProductsCrudApi.Products.Model;
using ProductsCrudApi.Products.Model.Comparers;
using ProductsCrudApi.Products.Service.Interfaces;
using ProductsCrudApi.System.Constants;
using ProductsCrudApi.System.Exceptions;
using ProductsCrudApi.Products.Controller;
using Microsoft.AspNetCore.Mvc;
using Moq;
using tests.Products.Helper;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Core;

namespace tests.Products.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<IProductQuerryService> _mockQueryService;
    private readonly Mock<IProductComandService> _mockCommandService;
    private readonly Mock<ILogger<ProductsController>> _mockLogger;
    private readonly ProductApiController _controller;

    public ProductControllerTests()
    {
        _mockQueryService = new Mock<IProductQuerryService>();
        _mockCommandService = new Mock<IProductComandService>();
        _mockLogger = new Mock<ILogger<ProductsController>>();
        _controller = new ProductsController(_mockLogger.Object, _mockQueryService.Object, _mockCommandService.Object);
       
    }

    [Fact]
    public async Task GetProducts_NoProductsExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetAllProducts())
            .ThrowsAsync(new ItemsDoNotExist(Constants.NO_PRODUCTS_EXIST));

        var result = await _controller.GetProducts();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Constants.NO_PRODUCTS_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetProducts_WhenProductsExist_ReturnsOkWithProducts()
    {
        var products = TestProductFactory.CreateProducts(2);
        _mockQueryService.Setup(service => service.GetAllProducts()).ReturnsAsync(products);

        var result = await _controller.GetProducts();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_InvalidPrice_ReturnsBadRequest()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "New Product",
            Price = 100,
            Category = "Test Category",
            DateOfFabrication = DateTime.UtcNow
        };

        _mockCommandService.Setup(service => service.CreateProduct(It.IsAny<CreateProductRequest>()))
            .ThrowsAsync(new InvalidPrice(Constants.INVALID_PRICE));

        var result = await _controller.CreateProduct(createRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(Constants.INVALID_PRICE, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ProductWithSameNameAlreadyExists_ReturnsBadRequest()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "New Product",
            Price = 100,
            Category = "Test Category",
            DateOfFabrication = DateTime.UtcNow
        };

        _mockCommandService.Setup(service => service.CreateProduct(It.IsAny<CreateProductRequest>()))
            .ThrowsAsync(new ItemAlreadyExists(Constants.PRODUCT_ALREADY_EXISTS));

        var result = await _controller.CreateProduct(createRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(Constants.PRODUCT_ALREADY_EXISTS, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ValidData_ReturnsOkWithProduct()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "New Product",
            Price = 100,
            Category = "Test Category",
            DateOfFabrication = DateTime.UtcNow
        };

        Product product = TestProductFactory.CreateProduct(1);
        product.Name = createRequest.Name;
        product.Price = createRequest.Price;
        product.Category = createRequest.Category;
        product.DateOfFabrication = createRequest.DateOfFabrication;

        _mockCommandService.Setup(service => service.CreateProduct(It.IsAny<CreateProductRequest>()))
            .ReturnsAsync(product);

        var result = await _controller.CreateProduct(createRequest);

        var okResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(product, okResult.Value as Product, new ProductEqualityComparer()!);
        Assert.Equal(201, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_InvalidPrice_ReturnsBadRequest()
    {
        var updateRequest = new UpdateProductRequest
        {
            Id = 1,
            Name = "New Product",
            Price = -100,
            Category = "Test Category",
            DateOfFabrication = DateTime.UtcNow
        };

        _mockCommandService.Setup(service => service.UpdateProduct(It.IsAny<UpdateProductRequest>()))
            .ThrowsAsync(new InvalidPrice(Constants.INVALID_PRICE));

        var result = await _controller.UpdateProduct(updateRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(Constants.INVALID_PRICE, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        var updateRequest = new UpdateProductRequest
        {
            Id = 1,
            Name = "New Product",
            Price = 100,
            Category = "Test Category",
            DateOfFabrication = DateTime.UtcNow
        };

        _mockCommandService.Setup(repo => repo.UpdateProduct(updateRequest))
            .ThrowsAsync(new ItemDoesNotExist(Constants.PRODUCT_DOES_NOT_EXIST));

        var result = await _controller.UpdateProduct(updateRequest);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Constants.PRODUCT_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ValidData_ReturnsOkWithProduct()
    {
        DateTime time = DateTime.Now;
        var updateRequest = new UpdateProductRequest
        {
            Id = 1,
            Name = "New Product",
            Price = 100,
            Category = "Test Category",
            DateOfFabrication = time
        };

        var product = TestProductFactory.CreateProduct(1);
        product.Name = updateRequest.Name;
        product.Price = 100;
        product.Category = "Test Category";
        product.DateOfFabrication = time;
        
        _mockCommandService.Setup(service => service.UpdateProduct(It.IsAny<UpdateProductRequest>()))
            .ReturnsAsync(product);

        var result = await _controller.UpdateProduct(updateRequest);

        var okResult = Assert.IsType<AcceptedResult>(result.Result);
        Assert.Equal(product, okResult.Value as Product, new ProductEqualityComparer()!);
        Assert.Equal(202, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        _mockCommandService.Setup(repo => repo.DeleteProduct(It.IsAny<int>()))
            .ThrowsAsync(new ItemDoesNotExist(Constants.PRODUCT_DOES_NOT_EXIST));

        var result = await _controller.DeleteProduct(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(Constants.PRODUCT_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteProduct_ProductExists_ReturnsOkWithProduct()
    {
        Product product = TestProductFactory.CreateProduct(1);

        _mockCommandService.Setup(repo => repo.DeleteProduct(It.IsAny<int>()))
            .ReturnsAsync(product);


        var result = await _controller.DeleteProduct(1);
        Assert.IsType<OkObjectResult>(result.Result);

      

      


    }
}