using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
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
        readonly IAppSettings _appSettings;

        public BetEngine(IRepository<User> userRepository, IRepository<Bet> betRepository, IRepository<SpinHistory> spinRepository, IAppSettings appSettings)
        {
            _userRepository = userRepository;
            _betRepository = betRepository;
            _spinRepository = spinRepository;
            _appSettings = appSettings;
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

        public Bet PlaceBet(BetType betType, string userId, decimal amount) =>
        new()
        {
            Id = Guid.NewGuid().ToString(),
            Amount = amount,
            UserId = userId,
            BetType = betType,
            IsBetConcluded = false,
            IsBetWon = false
        };

        public async Task<IEnumerable<SpinHistory>> ShowPreviousSpins()
        {
            var getAllBetsHistoryQuery = "SELECT Id, BetType, SpinDate FROM SpinHistory";
            using var connection = new SqliteConnection(_appSettings.ConnectionString);

            return await _spinRepository.GetAllAsync(getAllBetsHistoryQuery);
        }

        public async Task<BetType> Spin()
        {
            await Task.Delay(_appSettings.TimeOfSpinInSeconds);
            return EnumExtensions.GetRandomEnumValue<BetType>();
        }

        public async Task UpdateBets(BetType betType)
        {
            var timeOfConclution = DateTime.UtcNow;

            var getAllUncocludedBetsParams = new { IsBetConcluded = false };
            var getAllUnconcludedBetsQuery = "SELECT * FROM Bets WHERE IsBetConcluded = @IsBetConcluded";
            var bets = await _betRepository.GetAllAsync(getAllUnconcludedBetsQuery, getAllUncocludedBetsParams);

            if (bets == null) throw new NotFoundException("No bets found");

            foreach (var bet in bets)
            {
                bet.IsBetConcluded = true;
                bet.IsBetWon = bet.BetType == betType;
                bet.TimeOfConclution = timeOfConclution;

                var betQuery = "UPDATE Bets SET IsBetConcluded = @IsBetConcluded, IsBetWon = @IsBetWon WHERE Id = @Id";
                await _betRepository.UpdateAsync(betQuery, bet);
            }

            var getAllWinningUsersQuery = "SELECT DISTINCT * FROM Users LEFT JOIN Bets ON Users.Id = Bets.UserId WHERE Bets.IsBetWon = @IsBetWon AND Users.Id = Bets.UserId  LIMIT 1";
            var getAllWinningUsersParams = new { IsBetWon = true };
            var winningUserAccounts = await _userRepository.GetAllAsync(getAllWinningUsersQuery, getAllWinningUsersParams);
            if (winningUserAccounts == null) return;
            winningUserAccounts = winningUserAccounts.Distinct().ToList();
            //bulk edit
            foreach (var userAccount in winningUserAccounts)
            {
                var userBets = bets.Where(x => x.UserId == userAccount.Id && x.IsBetWon && DateTime.UtcNow >= timeOfConclution);
                userAccount.Balance += userBets.Sum(x => CalculatePayout(x.BetType, x.Amount));

                var userQuery = "UPDATE Users SET Balance = @Balance WHERE Id = @Id";
                await _userRepository.UpdateAsync(userQuery, userAccount);
            }
        }
    }
}