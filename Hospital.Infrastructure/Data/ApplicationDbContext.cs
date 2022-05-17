using Hospital.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientRecord> PatientRecord { get; set; }
        public DbSet<PatientStatistics> PatientStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.SystemIdNumber);
                entity.HasIndex(e => e.OfficialIdNumber).IsUnique();

                entity
                    .HasMany(c => c.PatientRecords)
                    .WithOne(c => c.Patient)
                    .HasForeignKey(c => c.PatientId)
                    .IsRequired();
            });

            builder.Entity<PatientRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasOne(c => c.Patient)
                    .WithMany(c => c.PatientRecords)
                    .HasForeignKey(c => c.PatientId)
                    .IsRequired();
            });

            builder.Entity<PatientStatistics>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(nameof(PatientStatistics));
            });
        }
    }
}