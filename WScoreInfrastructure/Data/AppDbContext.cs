using Microsoft.EntityFrameworkCore;
using WScoreDomain.Entities;

namespace WScoreInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Checkin> Checkins => Set<Checkin>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("TB_USER");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v)
                    )
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(150)");
            });

            modelBuilder.Entity<Checkin>().ToTable("TB_CHECKIN");

            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v)
                    )
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(),
                        v => Guid.Parse(v)
                    )
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DataCheckin)
                    .IsRequired()
                    .HasColumnType("TIMESTAMP(7)");

                entity.Property(e => e.Humor).HasColumnType("NUMBER(10)").IsRequired();
                entity.Property(e => e.Sono).HasColumnType("NUMBER(10)").IsRequired();
                entity.Property(e => e.Foco).HasColumnType("NUMBER(10)").IsRequired();
                entity.Property(e => e.Score).HasColumnType("NUMBER(10)").IsRequired();
                entity.Property(e => e.Energia).HasColumnType("NUMBER(10)").IsRequired();
                entity.Property(e => e.CargaTrabalho).HasColumnType("NUMBER(10)").IsRequired();

                entity.Property(e => e.Feedback)
                    .HasColumnType("NVARCHAR2(500)");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Checkins)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
