using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ShowDbContext(DbContextOptions<ShowDbContext> options) : DbContext(options)
    {
        public DbSet<Show> Show { get; set; } = default!;
    }
}
