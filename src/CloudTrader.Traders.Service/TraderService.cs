using System.Threading.Tasks;
using CloudTrader.Traders.Models.Service;
using CloudTrader.Traders.Service.Exceptions;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<Trader> CreateTrader();

        Task<Trader> GetTrader(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly ITraderRepository _traderRepository;

        public TraderService(ITraderRepository traderRepository)
        {
            _traderRepository = traderRepository;
        }

        public async Task<Trader> CreateTrader()
        {
            var trader = new Trader();

            trader = await _traderRepository.SaveTrader(trader);

            return trader;
        }

        public async Task<Trader> GetTrader(int id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }

            return trader;
        }
    }
}
