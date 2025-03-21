using choosing.Domain;
using Microsoft.EntityFrameworkCore;

namespace choosing.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Guest> Guests { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasKey(e => e.Dni);

                entity.ToTable("invitados");

                entity.Property(e => e.Dni)
                    .ValueGeneratedNever()
                    .HasColumnName("dni");
                entity.Property(e => e.Acreditado).HasColumnName("acreditado");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(255)
                    .HasColumnName("apellido");
                entity.Property(e => e.CantEntradas).HasColumnName("cant_entradas");
                entity.Property(e => e.DayOne)
                    .HasMaxLength(255)
                    .HasColumnName("day_one");
                entity.Property(e => e.DayTwo)
                    .HasMaxLength(255)
                    .HasColumnName("day_two");
                entity.Property(e => e.InfoAdicional)
                    .HasMaxLength(255)
                    .HasColumnName("info_adicional");
                entity.Property(e => e.Mail)
                    .HasMaxLength(255)
                    .HasColumnName("mail");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .HasColumnName("nombre");
            });

        }

    }
}
