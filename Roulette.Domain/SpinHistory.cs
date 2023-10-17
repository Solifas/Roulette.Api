using System;

namespace Roulette.Domain
{
    public class SpinHistory
    {
        public string Id { get; set; }
        public BetType BetType { get; set; }
        public DateTime SpinDate { get; set; }
    }
}