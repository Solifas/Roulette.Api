using System;

namespace Roulette.Application.Commands.Payout
{
    public class PayoutResponse
    {
        public Guid BetId { get; set; }
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
    }
}