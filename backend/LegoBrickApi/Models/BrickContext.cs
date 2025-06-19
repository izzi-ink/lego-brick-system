using Microsoft.EntityFrameworkCore;

namespace LegoBrickApi.Models
{
    public class BrickContext : DbContext
    {
        public BrickContext(DbContextOptions<BrickContext> options) : base(options)
        {
        }
        
        // This represents your Bricks table
        public DbSet<Brick> Bricks { get; set; }
    }
}