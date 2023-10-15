using System;

namespace Roulette.Domain
{
    public class SpinHistory
    {
        public Guid Id { get; set; }
        public DateTime SpinDate { get; set; }
        public BetType BetType { get; set; }
    }
}
