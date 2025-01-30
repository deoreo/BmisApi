using BmisApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Blotter> Blotters { get; set; }
        public DbSet<BrgyProject> BrgyProjects { get; set; }
        public DbSet<Official> Officials { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Vawc> Vawcs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<Sex>();

            modelBuilder.Entity<Resident>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("resident_pkeys");

                entity.ToTable("residents");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.Sex).IsRequired().HasConversion<string>();
                entity.Property(e => e.Birthday).IsRequired();
                entity.Property(e => e.Occupation);
                entity.Property(e => e.RegisteredVoter).IsRequired();
                entity.Property(e => e.IsHouseholdHead).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.LastUpdatedAt);
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Household>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("household_pkeys");

                // Members connection
                entity.HasMany(h => h.Members)
                .WithOne(r => r.Household)
                .HasForeignKey(r => r.HouseholdId);

                entity.ToTable("households");

                entity.Property(e => e.Id).IsRequired();  
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.HeadId);
                entity.Property(e => e.LastUpdatedAt);
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Blotter>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("blotter_pkeys");

                entity.HasOne(b => b.Complainant)
                      .WithMany()
                      .HasForeignKey(b => b.ComplainantId);

                entity.HasOne(b => b.Defendant)
                      .WithMany()
                      .HasForeignKey(b => b.DefendantId);

                entity.ToTable("blotters");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ComplainantId).IsRequired();
                entity.Property(e => e.DefendantId).IsRequired();
                entity.Property(e => e.Nature).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();
                entity.Property(e => e.Narrative).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<BrgyProject>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("project_pkeys");

                entity.ToTable("projects");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.ReferenceCode).IsRequired();
                entity.Property(e => e.ImplementingAgency).IsRequired();
                entity.Property(e => e.StartingDate).IsRequired();
                entity.Property(e => e.CompletionDate).IsRequired();
                entity.Property(e => e.ExpectedOutput).IsRequired();
                entity.Property(e => e.FundingSource).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Official>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("official_pkeys");

                entity.HasOne(o => o.Resident)
                    .WithMany()
                    .HasForeignKey(o => o.ResidentId);

                entity.ToTable("officials");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Position).IsRequired();
                entity.Property(e => e.Title);
                entity.Property(e => e.ResidentId).IsRequired();
                entity.Property(e => e.TermStart).IsRequired();
                entity.Property(e => e.TermEnd).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("incident_pkeys");

                entity.HasOne(i => i.Complainant)
                      .WithMany()
                      .HasForeignKey(i => i.ComplainantId);

                entity.ToTable("incidents");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ComplainantId).IsRequired();
                entity.Property(e => e.Nature).IsRequired();
                entity.Property(e => e.Narrative).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });

            modelBuilder.Entity<Vawc>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("vawc_pkeys");

                entity.HasOne(b => b.Complainant)
                      .WithMany()
                      .HasForeignKey(b => b.ComplainantId);

                entity.HasOne(b => b.Defendant)
                      .WithMany()
                      .HasForeignKey(b => b.DefendantId);

                entity.ToTable("vawc");

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ComplainantId).IsRequired();
                entity.Property(e => e.DefendantId).IsRequired();
                entity.Property(e => e.Nature).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();
                entity.Property(e => e.Narrative).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);

                entity.HasQueryFilter(x => x.DeletedAt == null);
            });
        }
    }
}
