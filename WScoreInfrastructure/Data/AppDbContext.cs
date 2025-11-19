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
            // USER
            modelBuilder.Entity<User>().ToTable("TB_USER");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });

            // CHECKIN
            modelBuilder.Entity<Checkin>().ToTable("TB_CHECKIN");
            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DataCheckin).IsRequired();
                entity.Property(e => e.Humor).IsRequired();
                entity.Property(e => e.Sono).IsRequired();
                entity.Property(e => e.Foco).IsRequired();
                entity.Property(e => e.CargaTrabalho).IsRequired();
                entity.Property(e => e.Score).IsRequired();

                // Novo campo Energia
                entity.Property(e => e.Energia).HasColumnType("NUMBER(10)");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Checkins)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
