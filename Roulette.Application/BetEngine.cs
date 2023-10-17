using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;
using Roulette.Infrastructure.Helpers;

namespace Roulette.Application
{
    public class BetEngine : IBetEngine
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Bet> _betRepository;
        private readonly IRepository<SpinHistory> _spinRepository;

        public BetEngine(IRepository<User> userRepository, IRepository<Bet> betRepository, IRepository<SpinHistory> spinRepository)
        {
            _userRepository = userRepository;
            _betRepository = betRepository;
            _spinRepository = spinRepository;
        }
        public decimal CalculatePayout(BetType betType, decimal amount)
        {
            return betType switch
            {
                BetType.Red or BetType.Black => 35 * amount,// to be confirmed
                BetType.Odd or BetType.Even => amount * 2,// to be confirmed
                BetType.FirstHalf or BetType.SecondHalf => amount * 2,// to be confirmed
                BetType.FirstDozen or BetType.SecondDozen or BetType.ThirdDozen => amount * 3,// to be confirmed
                BetType.FirstColumn or BetType.SecondColumn or BetType.ThirdColumn => amount * 3,// to be confirmed
                BetType.Single => amount * 35,// to be confirmed
                BetType.Split => amount * 17,
                BetType.Corner => amount * 8,
                BetType.Street => amount * 11,
                BetType.DoubleStreet => amount * 5,
                BetType.TopLine => amount * 6,
                BetType.Trio => amount * 11,
                _ => throw new NotFoundException("Bet type not found"),
            };
        }

        public Bet PlaceBet(BetType betType, Guid userId, decimal amount) =>
        new()
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            UserId = userId,
            BetType = betType
        };

        public async Task<IEnumerable<SpinHistory>> ShowPreviousSpins()
        {
            var getAllBetsHistoryQuery = "SELECT * FROM SpinHistory";
            return await _spinRepository.GetAllAsync(getAllBetsHistoryQuery);
        }

        public async Task<BetType> Spin()
        {
            await Task.Delay(2000);
            return EnumExtensions.GetRandomEnumValue<BetType>();
        }

        public async Task UpdateBets(BetType betType)
        {
            var timeOfConclution = DateTime.UtcNow;

            var getAllUnconcludedBetsQuery = "SELECT * FROM Bets WHERE IsBetConcluded = @IsBetConcluded";

            var bets = await _betRepository.GetAllAsync(getAllUnconcludedBetsQuery);

            if (bets == null) throw new NotFoundException("No bets found");

            foreach (var bet in bets)
            {
                bet.IsBetConcluded = true;
                bet.IsBetWon = bet.BetType == betType;
                bet.TimeOfConclution = timeOfConclution;

                var betQuery = "UPDATE Bets SET IsBetConcluded = @IsBetConcluded, IsBetWon = @IsBetWon, TimeOfConclution = @TimeOfConclution WHERE Id = @Id";
                await _betRepository.UpdateAsync(betQuery, bet);
            }

            var getAllWinningUsersQuery = "SELECT * FROM Users LEFT JOIN Bets ON Users.UserId = Bets.UserId WHERE Bets.IsBetWon = @IsBetWon";
            var winningUserAccounts = await _userRepository.GetAllAsync(getAllWinningUsersQuery);

            if (winningUserAccounts == null) throw new NotFoundException("No winners found");
            //bulk edit
            foreach (var userAccount in winningUserAccounts)
            {
                var userBets = bets.Where(x => x.UserId == userAccount.UserId && x.TimeOfConclution == timeOfConclution);
                userAccount.Balance += userBets.Sum(x => CalculatePayout(x.BetType, x.Amount));

                var userQuery = "UPDATE Users SET Balance = @Balance WHERE UserId = @UserId";
                await _userRepository.UpdateAsync(userQuery, userAccount);
            }
        }
    }
}