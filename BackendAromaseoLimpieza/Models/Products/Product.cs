using System.ComponentModel.DataAnnotations;

namespace BackendAromaseoLimpieza.Models.Products;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Presentation { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
}