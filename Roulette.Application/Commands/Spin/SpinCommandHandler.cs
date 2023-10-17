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
                var spinHistory = new SpinHistory
                {
                    BetType = betType,
                    Id = Guid.NewGuid(),
                    SpinDate = DateTime.UtcNow
                };
                string saveSpinQuery = "INSERT INTO SpinHistory (Id, BetType, SpinDate) VALUES (@Id, @BetType, @SpinDate)";
                _spinHistoryRepository.AddAsync(saveSpinQuery, spinHistory);

                _betEngine.UpdateBets(betType);

                return betType;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error spining the wheel.", ex);
            }
        }
    }
}

