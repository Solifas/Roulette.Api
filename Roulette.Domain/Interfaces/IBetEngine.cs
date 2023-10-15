using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roulette.Domain.Interfaces
{
    public interface IBetEngine
    {
        Task<BetType> Spin();
        decimal CalculatePayout(BetType betType, decimal amount);
        Bet PlaceBet(BetType betType, string userId, decimal amount);
        Task<List<BetHistory>> ShowPreviousSpins(string userId);
        Task UpdateBets(BetType number);
    }
}

