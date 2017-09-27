namespace FootballBetting.Models
{
    public class BetGame
    {
        public int GameId { get; set; }

        public Game Game { get; set; }

        public int BetId { get; set; }

        public Bet Bet { get; set; }

        public int PredictionId { get; set; }

        public ResultPrediction Prediction { get; set; }
    }
}
