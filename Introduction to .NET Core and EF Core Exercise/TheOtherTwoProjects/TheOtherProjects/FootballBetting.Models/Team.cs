namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Logo { get; set; }

        [Range(3,3)]
        public string Initials { get; set; }

        public int PrimaryKitColorId { get; set; }

        public Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }

        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public decimal Budged { get; set; }

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();

        public ICollection<Game> HomeTeams { get; set; } = new HashSet<Game>();

        public ICollection<Game> AwayTeams { get; set; } = new HashSet<Game>();
    }
}
