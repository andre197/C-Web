namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class Position
    {
        public string Id { get; set; }

        public string PositionDescription { get; set; }

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }
}
