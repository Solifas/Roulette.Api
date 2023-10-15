using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;

namespace Roulette.Application.Commands.Spin
{
    public class SpinCommandHandler : IRequestHandler<SpinCommand, BetType>
    {
        private readonly IBetEngine _betEngine;
        private readonly IRepository<SpinHistory> _spinHistoryRepository;
        private readonly ILogger _logger;

        public SpinCommandHandler(IBetEngine betEngine, IRepository<SpinHistory> spinHistoryRepository)
        {
            _betEngine = betEngine;
            _spinHistoryRepository = spinHistoryRepository;
        }

        public async Task<BetType> Handle(SpinCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var betType = await _betEngine.Spin();

                _ = _spinHistoryRepository.AddAsync(new SpinHistory
                {
                    BetType = betType,
                    Id = Guid.NewGuid(),
                    SpinDate = DateTime.UtcNow
                }, "");

                _ = _betEngine.UpdateBets(betType);

                return betType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error spinning the Roulette");
                throw;
            }
        }
    }
}

