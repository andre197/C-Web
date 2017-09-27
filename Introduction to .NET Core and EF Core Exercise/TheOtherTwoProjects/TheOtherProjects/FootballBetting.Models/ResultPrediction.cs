namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class ResultPrediction
    {
        public int Id { get; set; }

        public PredictionType Predicion { get; set; }

        public ICollection<BetGame> BetGames { get; set; } = new HashSet<BetGame>();
    }
}
