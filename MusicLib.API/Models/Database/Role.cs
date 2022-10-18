using System.ComponentModel.DataAnnotations;

namespace MusicLib.API.Models.Database;

public class Role
{
    [Key]
    public int ID { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;

    [Required]
    public string Name { get; set; }

    private List<User> Users { get; set; }
}