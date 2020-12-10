using CloudTrader.Traders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<Trader> CreateTrader(int initialBalance);

        Task<Trader> GetTrader(Guid id);

        Task<List<Trader>> GetTraders();

        Task<IEnumerable<TraderCloudStock>> GetTradersByMineId(Guid mineId);

        Task<ICollection<CloudStock>> GetTraderMines(Guid traderId);

        Task<Trader> SetBalance(Guid traderId, int newBalance);

        Task<Trader> UpdateBalance(Guid traderId, int amountToAdd);

        Task<CloudStock> GetTraderMine(Guid traderId, Guid mineId);

        Task<ICollection<CloudStock>> SetTraderMine(Guid traderId, Guid mineId, int newStock);

        Task<Trader> DeleteTraderMine(Guid id, Guid mineId);
    }
}