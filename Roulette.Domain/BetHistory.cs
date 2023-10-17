using System;

namespace Roulette.Domain
{
    public class BetHistory
    {
        public string UserId { get; set; }
        public string BetId { get; set; }

        public virtual Bet Bet { get; set; }
        public virtual User UserAccount { get; set; }
    }
}