using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Commands.PlaceBet;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;
namespace Roulette.Application.Commands.PlaceBet
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand>
    {
        private readonly IBetEngine _betEngine;
        private readonly IRepository<Bet> _betRepository;
        private readonly IRepository<BetHistory> _betHistoryRepository;
        private readonly IRepository<User> _userRepository;

        public PlaceBetCommandHandler(IBetEngine betEngine, IRepository<BetHistory> betHistoryRepository, IRepository<Bet> betRepository, IRepository<User> userRepository)
        {
            _betEngine = betEngine;
            _betHistoryRepository = betHistoryRepository;
            _betRepository = betRepository;
            _userRepository = userRepository;
        }
        public async Task Handle(PlaceBetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // validation here
                var validator = await new PlaceBetValidator().ValidateAsync(request, cancellationToken);
                if (validator.Errors.Count > 0) throw new ValidationException(validator);
                // Query to get a user with the userName below
                var getUser = "SELECT * FROM Users where id = @id";
                var user = await _userRepository.Get(getUser);

                var bet = _betEngine.PlaceBet(request.BetType, user.UserId, request.Amount);
                var betHistory = new BetHistory
                {
                    UserId = user.UserId,
                    BetId = bet.Id,
                };

                string betQuery = "INSERT INTO Bet (Id, Amount, UserId, BetType) VALUES (@Id, @Amount, @UserId, @BetType)";
                string betHistoryQuery = "INSERT INTO BetHistory (Id, UserId, BetId) VALUES (@Id, @UserId, @BetId)";
                await _betRepository.AddAsync(betQuery, bet);
                await _betHistoryRepository.AddAsync(betHistoryQuery, betHistory);

            }
            catch (Exception ex)
            {
                throw new Exception("There was an error placing the bet", ex);
            }
        }
    }
}