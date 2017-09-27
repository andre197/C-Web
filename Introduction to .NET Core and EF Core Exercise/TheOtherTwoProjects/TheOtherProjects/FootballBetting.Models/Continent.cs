namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class Continent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Country> Countries { get; set; } = new HashSet<Country>();
    }
}
