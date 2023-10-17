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
                var validator = await new PlaceBetValidator().ValidateAsync(request, cancellationToken);
                if (validator.Errors.Count > 0) throw new ValidationException(validator);
                var getUserParams = new { UserName = request.UserName };
                var getUser = $"SELECT Id, UserName, Balance FROM Users where UserName = @UserName";
                var user = await _userRepository.Get(getUser, getUserParams);
                if (user == null) throw new NotFoundException("The username entered is not valid");

                if (request.Amount > user.Balance) throw new BadRequestException("The bet exceeds your amount");
                var bet = _betEngine.PlaceBet(request.BetType, user.Id, request.Amount);
                var betHistory = new BetHistory
                {
                    UserId = user.Id,
                    BetId = bet.Id,
                };

                string betQuery = "INSERT INTO Bets (Id, Amount,IsBetConcluded, IsBetWon, UserId, BetType) VALUES (@Id, @Amount,@IsBetConcluded ,@IsBetWon, @UserId, @BetType)";
                string betHistoryQuery = "INSERT INTO BetHistory (UserId, BetId) VALUES (@UserId, @BetId)";
                await _betRepository.AddAsync(betQuery, bet);
                await _betHistoryRepository.AddAsync(betHistoryQuery, betHistory);

                UpdateUserBalance(user, bet.Amount);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error placing the bet", ex);
            }
        }

        private async Task UpdateUserBalance(User user, decimal betAmount)
        {
            var updateUserQuery = "UPDATE Users SET Balance = @Balance WHERE Id = @Id";
            user.Balance -= betAmount;
            _userRepository.UpdateAsync(updateUserQuery, user);
        }
    }
}