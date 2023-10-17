using System;
using MediatR;
using Roulette.Domain;
namespace Roulette.Application.Commands.PlaceBet
{
    public class PlaceBetCommand : IRequest
    {
        public string UserName { get; set; }
        public BetType BetType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeOfBet { get; set; } = DateTime.UtcNow;
    }
}

