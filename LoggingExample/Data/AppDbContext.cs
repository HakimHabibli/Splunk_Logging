using LoggingExample.Models;
using Microsoft.EntityFrameworkCore;

namespace LoggingExample.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }


}
