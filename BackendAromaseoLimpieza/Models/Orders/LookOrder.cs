namespace BackendAromaseoLimpieza.Models.Orders;

public class LookOrder
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Names { get; set; }
    public string LastNames { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; }
    public List<LookDetailOrder> LookDetailOrders { get; set; }
}