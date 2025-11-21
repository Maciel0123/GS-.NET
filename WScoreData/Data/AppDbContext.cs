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

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("NVARCHAR2(150)");
            });

            modelBuilder.Entity<Checkin>().ToTable("TB_CHECKIN");
            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DataCheckin)
                    .IsRequired()
                    .HasColumnType("TIMESTAMP(7)");

                entity.Property(e => e.Humor)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.Sono)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.Foco)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.Score)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.Energia)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.CargaTrabalho)
                    .IsRequired()
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.Feedback)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("NVARCHAR2(500)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("RAW(16)");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Checkins)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
