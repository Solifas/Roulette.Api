using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Commands.PlaceBet;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;
namespace Roulette.Application.Commands.SingleBets
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand>
    {
        private readonly IBetEngine _betEngine;
        private readonly IRepository<Bet> _betRepository;
        private readonly IRepository<BetHistory> _betHistoryRepository;

        public PlaceBetCommandHandler(IBetEngine betEngine)
        {
            _betEngine = betEngine;
        }
        public async Task Handle(PlaceBetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // validation here
                var bet = _betEngine.PlaceBet(request.BetType, request.UserId, request.Amount);
                var betHistory = new BetHistory
                {
                    UserId = request.UserId,
                    BetId = bet.Id,
                };

                await _betRepository.AddAsync(bet, "");
                await _betHistoryRepository.AddAsync(betHistory, "");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}