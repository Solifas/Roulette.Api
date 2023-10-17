using System;
namespace Roulette.Domain
{
    public class Bet
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsBetConcluded { get; set; }
        public bool IsBetWon { get; set; }
        public string UserId { get; set; }
        public DateTime TimeOfConclution { get; set; }
        public BetType BetType { get; set; }
        public DateTime TimeOfBet { get; set; } = DateTime.UtcNow;
    }

    public class BetResult
    {
        public decimal AmountWon { get; set; }
    }
}