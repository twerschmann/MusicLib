using System.ComponentModel.DataAnnotations;

namespace MusicLib.API.Models.Database;

public class User
{
    [Required]
    [Key]
    public int ID { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required]
    public string Email { get; set; }

    public string Token { get; set; }
}