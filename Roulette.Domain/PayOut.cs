using System;
namespace Roulette.Domain
{
    public class PayOut
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsRetreived { get; set; }
        public DateTime TimeOfWithdrawal { get; set; }
    }
}