using Microsoft.EntityFrameworkCore;
using Proyecto_1.Models;

namespace Proyecto_1.Data
{
    public class SelvaDbContext: DbContext
    {
        public SelvaDbContext(DbContextOptions<SelvaDbContext> options) : base(options) { }
        public DbSet<Cuidadores> Cuidadores { get; set; }
    }
}
