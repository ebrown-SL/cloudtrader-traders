using CloudTrader.Traders.Models.Data;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<TraderDbModel> SaveTrader(TraderDbModel trader);

        Task<TraderDbModel> GetTrader(int id);
    }
}
