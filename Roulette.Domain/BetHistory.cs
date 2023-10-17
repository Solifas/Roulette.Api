using System;

namespace Roulette.Domain
{
    public class BetHistory
    {
        public Guid UserId { get; set; }
        public Guid BetId { get; set; }

        public virtual Bet Bet { get; set; }
        public virtual User UserAccount { get; set; }
    }
}