using BmisApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Household> Households { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setup soft delete
            modelBuilder.Entity<Resident>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Household>().HasQueryFilter(x => x.DeletedAt == null);

            modelBuilder.Entity<Resident>(entity =>
            {
                entity.HasKey(e => e.ResidentId).HasName("resident_pkeys");

                entity.ToTable("residents");

                entity.Property(e => e.ResidentId).IsRequired();
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.Sex).IsRequired().HasConversion<int>();
                entity.Property(e => e.Birthday).IsRequired();
                entity.Property(e => e.Occupation);
                entity.Property(e => e.RegisteredVoter).IsRequired();
                entity.Property(e => e.IsHouseholdHead).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.LastUpdatedAt);
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);
            });

            modelBuilder.Entity<Household>(entity =>
            {
                entity.HasKey(e => e.HouseholdId).HasName("household_pkeys");

                // Members connection
                entity.HasMany(h => h.Members)
                .WithOne(r => r.Household)
                .HasForeignKey(r => r.HouseholdId);

                // Head connection
                //entity.HasOne(h => h.Head)
                //.WithOne()
                //.HasForeignKey<Household>(h => h.HeadId);

                entity.ToTable("households");

                entity.Property(e => e.HouseholdId).IsRequired();  
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.HeadId);
                entity.Property(e => e.LastUpdatedAt);
                entity.Property(e => e.DeletedAt).HasDefaultValue(null);
            });
        }
    }
}
