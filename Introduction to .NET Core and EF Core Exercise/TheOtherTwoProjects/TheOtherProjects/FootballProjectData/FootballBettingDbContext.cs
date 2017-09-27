namespace FootballProjectData
{
    using FootballBetting.Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballBettingDbContext : DbContext
    {
        public DbSet<Continent> Continents { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<CompetitionType> CompetitionTypes { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<ResultPrediction> ResultPredictions { get; set; }

        public DbSet<BetGame> BetGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=LAPTOP-22QSF26P\SQLEXPRESS;Database=TestDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>()
                .HasOne(c => c.Continent)
                .WithMany(co => co.Countries)
                .HasForeignKey(c => c.ContinentId);

            builder.Entity<Town>()
                .HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CountryId);

            builder.Entity<Team>()
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);

            builder.Entity<Team>()
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryColorTeams)
                .HasForeignKey(t => t.PrimaryKitColorId);

            builder.Entity<Team>()
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryColorTeams)
                .HasForeignKey(t => t.SecondaryKitColorId);

            builder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            builder.Entity<Player>()
                .HasOne(p => p.Position)
                .WithMany(pos => pos.Players)
                .HasForeignKey(p => p.PositionId);

            builder.Entity<Competition>()
                .HasOne(c => c.CompetitionType)
                .WithMany(ct => ct.Competitions)
                .HasForeignKey(c => c.CompetitionTypeId);

            builder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

            builder.Entity<Game>()
                .HasOne(g => g.Round)
                .WithMany(r => r.Games)
                .HasForeignKey(g => g.RoundId);

            builder.Entity<Game>()
                .HasOne(g => g.Competition)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CompetitionId);

            builder.Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeTeams)
                .HasForeignKey(g => g.HomeTeamId);

            builder.Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayTeams)
                .HasForeignKey(g => g.AwayTeamId);


            builder.Entity<BetGame>()
                .HasKey(bg => new { bg.GameId, bg.BetId });

            builder.Entity<BetGame>()
                .HasOne(bg => bg.Bet)
                .WithMany(b => b.BetGames)
                .HasForeignKey(bg => bg.BetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BetGame>()
                .HasOne(bg => bg.Game)
                .WithMany(g => g.BetGames)
                .HasForeignKey(bg => bg.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BetGame>()
                .HasOne(bg => bg.Prediction)
                .WithMany(p => p.BetGames)
                .HasForeignKey(bg => bg.PredictionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.GameId, ps.PlayerId });

            builder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
