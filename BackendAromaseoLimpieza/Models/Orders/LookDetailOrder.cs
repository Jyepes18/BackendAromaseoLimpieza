namespace BackendAromaseoLimpieza.Models.Orders;

public class LookDetailOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductPresentation { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}