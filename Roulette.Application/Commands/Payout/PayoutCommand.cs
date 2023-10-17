using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Roulette.Application.Commands.Payout
{
    public class PayoutCommand : IRequest<PayoutResponse>
    {
        public Guid BetId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
    }
}