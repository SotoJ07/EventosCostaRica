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
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Asiento> Asientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().Property(u => u.Apellidos).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().Property(u => u.Correo).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
            modelBuilder.Entity<Usuario>().Property(u => u.ContraseñaHash).IsRequired();
            modelBuilder.Entity<Usuario>().Property(u => u.Estado).HasDefaultValue(true);
            modelBuilder.Entity<Usuario>().Property(u => u.FechaRegistro).HasDefaultValueSql("GETDATE()");

            // Relación Usuario → Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Rol
            modelBuilder.Entity<Rol>().Property(r => r.Nombre).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Asiento>()
        .HasOne(a => a.Entrada)
        .WithMany(e => e.Asientos)
        .HasForeignKey(a => a.EntradaId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}