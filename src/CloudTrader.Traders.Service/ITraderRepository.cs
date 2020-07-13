using CloudTrader.Traders.Models.Service;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task<Trader> SaveTrader(Trader trader);

        Task<Trader> GetTrader(int id);
    }
}
