using CloudTrader.Traders.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<TraderDbModel> SaveTrader(TraderDbModel trader);
        Task<TraderDbModel> GetTrader(int id);
        Task<List<TraderDbModel>> GetTraders();
        Task<TraderDbModel> SetBalance(int id, int balance);
        Task<TraderDbModel> AddTraderMine(int id, int mineId);
    }
}
