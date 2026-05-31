using BackendAromaseoLimpieza.Enums;
using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Orders;
using Dapper;
using Npgsql;

namespace BackendAromaseoLimpieza.Services;

public class OrderService : IOrderService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    
    public OrderService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Connection string 'DefaultConnection' not found in configuration");
    }

    public async Task<Result<string, int>> CreateOrder(Order order)
    {
        try
        {
            // 1 first create the order and get the id
            string insertOrder = @"
                insert into pedido (usuario_id, estado, dia_entrega)
                values (@usuario_id, @estado, @dia_entrega)
                returning id;
            ";
            
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            int orderId = await connection.ExecuteScalarAsync<int>(insertOrder, new
            {
                usuario_id = order.UserId,
                estado = StatusOrder.PROGRAMMED.ToString(), 
                dia_entrega = order.Date
            });
            
            if (orderId == 0)
                return Result<string, int>.Failure("Error creating order", 500);
            
            // 2 second create detail order according to id order
            foreach (var detailOrder in order.DetailOrders )
            {
                string insertDetailOrder = @"
                    insert into detalle_pedido (pedido_id, producto_id, cantidad)
                    values (@pedido_id, @producto_id, @cantidad);
                ";
                
                var result = await connection.ExecuteAsync(insertDetailOrder, new
                {
                    pedido_id = orderId,
                    producto_id = detailOrder.ProductId,
                    cantidad = detailOrder.Quantity
                });
                
                if (result > 0)
                    return Result<string, int>.Success("Order created successfully", 201);
                
                return Result<string, int>.Failure("Error creating order", 500);
            }
            
            await connection.DisposeAsync();
            
            return Result<string, int>.Success("Order created successfully", 201);
        }
        catch (Exception e)
        {
            throw new Exception("Error creating order ", e);
        }
    }

    public async Task<Result<LookOrder, int>> GetOrderById(int id)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            string selectOrder = @"
                select 
    	            p.id as Id,
    	            p.usuario_id as UserId,
    	            u.nombres as Names,
    	            p.estado as Status,
    	            u.apellidos as LastNames,
    	            p.dia_entrega as Date
                from pedido p 
                inner join usuario u on p.usuario_id = u.id 
                where p.id = @Id
            ";
            
            var order = await connection.QuerySingleOrDefaultAsync<LookOrder>(selectOrder, new { Id = id });
            
            if (order == null)
                return Result<LookOrder, int>.Failure("Order not found", 404);

            string detailOrder = @"
                select 
    	            dp.id as Id,
    	            dp.producto_id as ProductId,
    	            p.nombre as ProductName,
    	            p.presentacion as ProductPresentation,
    	            p.precio_unitario as ProductPrice,
    	            dp.cantidad as Quantity
                from detalle_pedido dp 
                inner join producto p on dp.producto_id = p.id 
                where dp.pedido_id = @Id
            ";
            
            var detailOrders = await connection.QueryAsync<LookDetailOrder>(detailOrder, new { Id = id });
            
            if(detailOrders == null)
                return Result<LookOrder, int>.Failure("Detail orders not found", 404);

            var orderSelect = new LookOrder
            {
                Id = order.Id,
                UserId = order.UserId,
                Names = order.Names,
                LastNames = order.LastNames,
                Status = order.Status,
                Date = order.Date,
                LookDetailOrders = detailOrders.Select(p => new LookDetailOrder
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductPresentation = p.ProductPresentation,
                    ProductPrice = p.ProductPrice,
                    Quantity = p.Quantity
                }).ToList()
            };
            
            return Result<LookOrder, int>.Success(orderSelect, 200);
        }
        catch (Exception e)
        {

            throw new Exception("error to get order by id ", e);
        }
    }
}