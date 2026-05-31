namespace BackendAromaseoLimpieza.Models.Orders;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<DetailOrder> DetailOrders { get; set; }
}