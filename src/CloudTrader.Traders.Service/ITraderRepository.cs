using CloudTrader.Traders.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<TraderDbModel> SaveTrader(TraderDbModel trader);
        Task<TraderDbModel> GetTrader(Guid id);
        Task<List<TraderDbModel>> GetTraders();
        Task<TraderDbModel> SetBalance(Guid id, int balance);
        Task<TraderDbModel> SetTraderMine(Guid id, int mineId, int stock);
        Task<TraderDbModel> DeleteTraderMine(Guid id, int mineId);
    }
}
