namespace BackendAromaseoLimpieza.Models.Orders;

public class DetailOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}