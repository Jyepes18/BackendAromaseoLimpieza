using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BackendAromaseoLimpieza.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var result = await _orderService.CreateOrder(order);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("get/{Id}")]
    public async Task<IActionResult> GetOrder([FromRoute] int Id)
    {
        var result = await _orderService.GetOrderById(Id);
        return Ok(result);
    }
}