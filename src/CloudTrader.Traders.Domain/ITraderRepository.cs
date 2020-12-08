using CloudTrader.Traders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<Trader> SaveTrader(Trader trader);

        Task<Trader> GetTrader(Guid id);

        Task<List<Trader>> GetTraders();

        Task<List<Trader>> GetTradersByMineId(Guid mineId);

        Task<Trader> SetBalance(Guid id, int balance);

        Task<Trader> UpdateBalance(Guid id, int amount);

        Task<Trader> SetTraderMine(Guid id, Guid mineId, int stock);

        Task<Trader> DeleteTraderMine(Guid id, Guid mineId);
    }
}