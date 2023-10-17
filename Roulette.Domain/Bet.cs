using System;
namespace Roulette.Domain
{
    public class Bet
    {

        public Guid Id { get; set; }
        public Decimal Amount { get; set; }
        public bool IsBetConcluded { get; set; }
        public bool IsBetWon { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeOfConclution { get; set; }
        public BetType BetType { get; set; }
        public DateTime TimeOfBet { get; set; } = DateTime.UtcNow;
    }

    public class BetResult
    {
        public decimal AmountWon { get; set; }
    }
}

