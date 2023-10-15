using System;

namespace Roulette.Domain
{
    public class BetHistory
    {
        public string UserId { get; set; }
        public Guid BetId { get; set; }

        public virtual Bet Bet { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}