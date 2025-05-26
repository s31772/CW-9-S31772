using Microsoft.EntityFrameworkCore;
using apbd_cw9.Models;

namespace apbd_cw9.Data;

public class AppDbContext : DbContext
{
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Prescription> Prescription { get; set; }
    public DbSet<Medicament> Medicament { get; set; }
    public DbSet<Prescription_Medicament> Prescription_Medicament { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdMedicament)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdPrescription)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
