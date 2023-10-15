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
        private readonly IRepository<UserAccount> _userRepository;
        private readonly IRepository<Bet> _betRepository;
        private readonly IRepository<BetHistory> _betHistoryRepository;

        public BetEngine(IRepository<UserAccount> userRepository, IRepository<Bet> betRepository)
        {
            _userRepository = userRepository;
            _betRepository = betRepository;
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
            Id = Guid.NewGuid(),
            Amount = amount,
            UserId = userId,
            BetType = betType
        };

        public async Task<List<BetHistory>> ShowPreviousSpins(string userId)
        {
            return (await _betHistoryRepository.GetAllAsync("bet")).Where(x => x.UserId == userId).ToList();
        }

        public async Task<BetType> Spin()
        {
            await Task.Delay(2000);
            return EnumExtensions.GetRandomEnumValue<BetType>();
        }

        public async Task UpdateBets(BetType betType)
        {
            var timeOfConclution = DateTime.UtcNow;
            var bets = (await _betRepository.GetAllAsync("")).Where(x => x.IsBetConcluded == false);
            foreach (var bet in bets)
            {
                bet.IsBetConcluded = true;
                bet.IsBetWon = bet.BetType == betType;
                bet.TimeOfConclution = timeOfConclution;
                await _betRepository.UpdateAsync("bet");
            }
            var userAccounts = (await _userRepository.GetAllAsync("")).Where(x=> x.Bets.Any(y=> y.IsBetWon == true));

            foreach (var userAccount in userAccounts)
            {
                var userBets = bets.Where(x => x.UserId == userAccount.UserId && x.TimeOfConclution == timeOfConclution);
                userAccount.Balance += userBets.Sum(x => CalculatePayout(x.BetType, x.Amount));
                await _userRepository.UpdateAsync("userAccount");
            }
        }
    }
}