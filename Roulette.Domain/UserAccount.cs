using System.Collections.Generic;

namespace Roulette.Domain
{
    public class UserAccount
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public BetType BetType { get; set; }
        public List<Bet> Bets { get; set; }
    }
}
