using System;
using System.Collections.Generic;

namespace Roulette.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}
