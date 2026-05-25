using System.ComponentModel.DataAnnotations;

namespace BackendAromaseoLimpieza.Models.Products;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Presentacion { get; set; }

    [Required]
    public decimal PrecioUnitario { get; set; }
}
