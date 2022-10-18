using Microsoft.EntityFrameworkCore;

namespace MusicLib.API.Models.Database;


/*
 * dotnet ef migrations add [Name]
 */


public class DatabaseContext : DbContext
{
    public DatabaseContext()
    { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    { }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=MUSICLIB_DEV;Trusted_Connection=True" );
    }

}