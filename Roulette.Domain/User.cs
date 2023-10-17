using System;
using System.Collections.Generic;

namespace Roulette.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public List<Bet> Bets { get; set; }
    }
}
