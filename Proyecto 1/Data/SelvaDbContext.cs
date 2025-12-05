using Microsoft.EntityFrameworkCore;
using Proyecto_1.Models;

namespace Proyecto_1.Data
{
    public class SelvaDbContext : DbContext
    {
        public SelvaDbContext(DbContextOptions<SelvaDbContext> options) : base(options) { }

        public DbSet<Cuidadores> Cuidadores { get; set; }
        public DbSet<Animal> Animales { get; set; }
        public DbSet<Visitantes> Visitantes { get; set; }
        public DbSet<MovimientoVisitas> MovimientoVisitas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones
            modelBuilder.Entity<MovimientoVisitas>()
                .HasOne(m => m.Visitante)
                .WithMany(v => v.MovimientoVisitas)
                .HasForeignKey(m => m.IdVisitante)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoVisitas>()
                .HasOne(m => m.Animal)
                .WithMany(a => a.MovimientoVisitas)
                .HasForeignKey(m => m.IdAnimal)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoVisitas>()
                .HasOne(m => m.Cuidador)
                .WithMany()
                .HasForeignKey(m => m.IdCuidador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Cuidador)
                .WithMany()
                .HasForeignKey(a => a.IdCuidador)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}