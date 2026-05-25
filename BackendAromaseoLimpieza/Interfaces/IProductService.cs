using BackendAromaseoLimpieza.Models.Products;

namespace BackendAromaseoLimpieza.Interfaces;

public interface IProductService
{
    Task<Result<string, int>> CreateProduct(Product product);
    Task<Result<Product, int>> GetProductById(int id);
    Task<Result<string, int>> UpdateProduct(Product product);
    Task<Result<object, int>> GetProducts(int page, int pageSize, ProductFilter filter);
}