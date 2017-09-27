namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int ContinentId { get; set; }

        public Continent Continent { get; set; }

        public ICollection<Town> Towns { get; set; } = new HashSet<Town>();
    }
}
