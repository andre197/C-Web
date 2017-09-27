﻿namespace FootballBetting.Models
{
    using System;
    using System.Collections.Generic;

    public class Bet
    {
        public int Id { get; set; }

        public decimal BetMoney { get; set; }

        public DateTime BetDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<BetGame> BetGames { get; set; } = new HashSet<BetGame>();
    }
}
