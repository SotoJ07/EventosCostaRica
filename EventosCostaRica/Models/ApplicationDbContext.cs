using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EventosCostaRica.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas (entidades)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().Property(u => u.Correo).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
            modelBuilder.Entity<Usuario>().Property(u => u.ContraseñaHash).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.Estado).HasDefaultValue(true); // Fix: Use HasDefaultValue instead of HasDefaultValueSql
            modelBuilder.Entity<Usuario>().Property(u => u.FechaRegistro).HasDefaultValueSql("GETDATE()"); // Ensure the correct SQL Server provider is referenced

            // Relación Usuario → Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Rol
            modelBuilder.Entity<Rol>().Property(r => r.Nombre).IsRequired().HasMaxLength(50);
        }
    }
}