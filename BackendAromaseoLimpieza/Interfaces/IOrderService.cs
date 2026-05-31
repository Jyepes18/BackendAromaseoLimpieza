using BackendAromaseoLimpieza.Models.Orders;

namespace BackendAromaseoLimpieza.Interfaces;

public interface IOrderService
{
    Task<Result<string, int>> CreateOrder(Order order);
    Task<Result<LookOrder, int>> GetOrderById(int id);
}