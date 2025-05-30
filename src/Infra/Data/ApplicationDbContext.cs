using AgendaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medico>()
                .HasMany(m => m.Consultas)
                .WithOne(c => c.Medico)
                .HasForeignKey(c => c.MedicoId);

            modelBuilder.Entity<Medico>()
                .HasIndex(m => m.Crm)
                .IsUnique();

            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.Consultas)
                .WithOne(c => c.Paciente)
                .HasForeignKey(c => c.PacienteId);
            modelBuilder.Entity<Consulta>()
                .Property(c => c.Status)
                .HasConversion<string>();

        }

    }
}