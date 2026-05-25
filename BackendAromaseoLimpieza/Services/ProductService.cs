using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Products;
using Dapper;
using Npgsql;

namespace BackendAromaseoLimpieza.Services;

public class ProductService : IProductService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public ProductService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Result<string, int>> CreateProduct(Product product)
    {
        try
        {
            string insertProduct = @"
                 insert into producto (nombre, precio_unitario, presentacion)
                 values (@nombre, @precio_unitario, @presentacion)
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.ExecuteAsync(insertProduct, new
            {
                nombre = product.Name,
                precio_unitario = product.UnitPrice,
                presentacion = product.Presentation
            });

            await connection.DisposeAsync();

            if (result > 0)
                return Result<string, int>.Success("Product created successfully", 201);

            return Result<string, int>.Failure("Error to create new product", 500);
        }
        catch (Exception e)
        {
            throw new Exception("Error to generate product ", e);
        }
    }

    public async Task<Result<Product, int>> GetProductById(int id)
    {
        try
        {
            string query = @"
                select 
                    p.id as Id,
                    p.nombre as Name,
                    p.precio_unitario as UnitPrice,
                    p.presentacion as Presentation
                from producto p where id = @id
            ";
            
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { id });
            
            await connection.DisposeAsync();
            
            if (product != null)
                return Result<Product, int>.Success(product, 200);
            
            return Result<Product, int>.Failure("Product not found", 404);
        }
        catch (Exception e)
        {
            throw new Exception("Error to get product by id", e);
        }
    }

    public async Task<Result<string, int>> UpdateProduct(Product product)
    {
        try
        {
            string updateProduct = @"
                update producto set 
                    nombre = @nombre, 
                    precio_unitario = @precio_unitario, 
                    presentacion = @presentacion 
                where id = @id
            ";
            
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var result = await connection.ExecuteAsync(updateProduct, new
            {
                id = product.Id,
                nombre = product.Name,
                precio_unitario = product.UnitPrice,
                presentacion = product.Presentation
            });
            
            await connection.DisposeAsync();
            
            if (result > 0)
                return Result<string, int>.Success("Product updated successfully", 200);
            
            return Result<string, int>.Failure("Product not found", 404);
        }
        catch (Exception e)
        {
            throw new Exception("Error to update product ", e);
        }
    }

    public async Task<Result<object, int>> GetProducts(int page, int pageSize, ProductFilter filter)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                select count(1)
                from producto p
                where
                    (COALESCE(@Name, '') = '' OR p.nombre ILIKE '%' || @Name || '%')
                    AND (COALESCE(@Presentation, '') = '' OR p.presentacion ILIKE '%' || @Presentation || '%');

                select
                    p.id as Id,
                    p.nombre as Name,
                    p.presentacion as Presentation,
                    p.precio_unitario as UnitPrice
                from producto p
                where
                    (COALESCE(@Name, '') = '' OR p.nombre ILIKE '%' || @Name || '%')
                    AND (COALESCE(@Presentation, '') = '' OR p.presentacion ILIKE '%' || @Presentation || '%')
                order by p.id desc
                OFFSET (@Page - 1) * @PageSize
                LIMIT @PageSize;
            ";

            var result = await connection.QueryMultipleAsync(query, new
            {
                Name = filter.Name,
                Presentation = filter.Presentation,
                Page = page,
                PageSize = pageSize
            });

            var numberOfRecords = result.Read<int>().Single();

            var collection = result.Read<Product>().ToList();

            return Result<object, int>.Success(new
            {
                data = collection,
                total = numberOfRecords
            }, 200);
        }
        catch (Exception e)
        {
            throw new Exception("error to get products ", e);
        }
    }
}