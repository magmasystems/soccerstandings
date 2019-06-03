using Microsoft.EntityFrameworkCore;
using SoccerStandings.Models;

namespace SoccerStandings.Persistence
{
    public class XMatchResultsContext : MagmaDbContext
    {
        public DbSet<MatchResults> MatchResults { get; set; }
        public DbSet<Round>        Rounds { get; set; }
        public DbSet<Match>        Matches { get; set; }
        public DbSet<Team>         Teams { get; set; }

        public XMatchResultsContext() : base("matches")
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MatchResults>(entity =>
            {
                entity.ToTable("MatchResults");
                entity.HasKey("Id");

                entity.Property(e => e.LeagueName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Round>(entity =>
            {
                // entity.HasKey(e => new { e.Id, e.MatchResultsId });
                entity.HasKey(e => e.Id);

                entity.Property(e => e.RoundName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.MatchResults)
                    .WithMany(p => p.Rounds)
                    .HasForeignKey(d => d.MatchResultsId)
                    .OnDelete(DeleteBehavior.Cascade)
                    ;
            });


            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.Cascade)
                    ;
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            /*
             * 
                entity.HasOne(d => d.ModifiedbyNavigation)
                    .WithMany(p => p.CompaniesModifiedByNavigation)
                    .HasForeignKey(d => d.Modifiedby)
                    .HasConstraintName("FK_Company_ModifiedBy");
             */

        }
    }
}
