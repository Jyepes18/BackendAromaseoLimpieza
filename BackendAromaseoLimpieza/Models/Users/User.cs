using System.ComponentModel.DataAnnotations;

namespace BackendAromaseoLimpieza.Models.Users;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Names { get; set; }
    [Required]
    public string LastNames { get; set; }
    public string? Company { get; set; }
    public string? Document { get; set; }
    [Required]
    public string Email { get; set; }
    public string? Password { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Location { get; set; }
    public int RoleId { get; set; } = 2;
}