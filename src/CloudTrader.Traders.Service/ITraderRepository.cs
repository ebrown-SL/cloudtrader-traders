using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public interface ITraderRepository
    {
        Task SaveTrader(Trader trader);

        Task<Trader> GetTrader(int id);
    }
}
