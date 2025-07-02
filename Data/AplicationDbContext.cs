using Microsoft.EntityFrameworkCore;
using TelemedicinaMonitores.Models;

namespace TelemedicinaMonitores.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<PatientData>(); // 👈 Esto evita que se intente mapear
        }
    }
}
