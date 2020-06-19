using System.Threading.Tasks;
using CloudTrader.Traders.Service.Exceptions;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<Trader> CreateTrader(int id);

        Task<Trader> GetTrader(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly ITraderRepository _traderRepository;

        public TraderService(ITraderRepository traderRepository)
        {
            _traderRepository = traderRepository;
        }

        public async Task<Trader> CreateTrader(int id)
        {
            var existingTrader = await _traderRepository.GetTrader(id);
            if (existingTrader != null)
            {
                throw new TraderAlreadyExistsException(id);
            }

            var trader = new Trader
            {
                Id = id
            };
            await _traderRepository.SaveTrader(trader);

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
