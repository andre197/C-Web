namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class CompetitionType
    {
        public int Id { get; set; }

        public TypeOfCompetition TypeOfCompetition { get; set; }

        public ICollection<Competition> Competitions { get; set; } = new HashSet<Competition>();
    }
}