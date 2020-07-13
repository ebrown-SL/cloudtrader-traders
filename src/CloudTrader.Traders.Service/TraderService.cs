using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Models.Service;
using CloudTrader.Traders.Service.Exceptions;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<Trader> CreateTrader();
        Task<Trader> GetTrader(int id);
        Task<List<Trader>> GetTraders();
    }

    public class TraderService : ITraderService
    {
        private readonly ITraderRepository _traderRepository;

        private readonly IMapper _mapper;

        public TraderService(ITraderRepository traderRepository, IMapper mapper)
        {
            _traderRepository = traderRepository;

            _mapper = mapper;
        }

        public async Task<Trader> CreateTrader()
        {
            var trader = await _traderRepository.SaveTrader(new TraderDbModel());

            return _mapper.Map<Trader>(trader);
        }

        public async Task<Trader> GetTrader(int id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }

            return _mapper.Map<Trader>(trader);
        }

        public async Task<List<Trader>> GetTraders()
        {
            var traders = await _traderRepository.GetTraders();
            return _mapper.Map<List<Trader>>(traders);
        }
    }
}
