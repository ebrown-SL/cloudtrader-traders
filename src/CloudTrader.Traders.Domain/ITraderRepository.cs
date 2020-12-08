using CloudTrader.Traders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<Trader> CreateTrader(Trader trader);

        Task<Trader> GetTrader(Guid id);

        Task<List<Trader>> GetTraders();

        Task<List<Trader>> GetTradersByMineId(Guid mineId);

        Task<Trader> UpdateTrader(Trader trader);
    }
}