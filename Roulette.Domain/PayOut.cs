using System;
namespace Roulette.Domain
{
    public class PayOut
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsRetreived { get; set; }
        public DateTime TimeOfWithdrawal { get; set; }
    }
}