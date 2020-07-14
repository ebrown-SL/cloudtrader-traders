using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudTrader.Traders.Models.Api;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Models.Service;
using CloudTrader.Traders.Service.Exceptions;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<TraderResponseModel> CreateTrader();
        Task<TraderResponseModel> GetTrader(int id);
        Task<GetAllTradersResponseModel> GetTraders();
        Task<TraderDbModel> GetTraderMines(int id);
        Task<TraderResponseModel> SetBalance(int id, int balance);
        Task<CloudStockDbModel> GetTraderMine(int id, int mineId);
        Task<List<CloudStockDbModel>> AddTraderMine(int id, AddTraderMineModel mine);
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

        public async Task<TraderResponseModel> CreateTrader()
        {
            var trader = await _traderRepository.SaveTrader(new TraderDbModel());

            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<TraderResponseModel> GetTrader(int id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }

            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<TraderDbModel> GetTraderMines(int id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            return trader;
        }

        public async Task<GetAllTradersResponseModel> GetTraders()
        {
            var traders = await _traderRepository.GetTraders();
            var mappedTraders = _mapper.Map<List<TraderResponseModel>>(traders);
            return new GetAllTradersResponseModel(mappedTraders);
        }

        public async Task<TraderResponseModel> SetBalance(int id, int balance)
        {
            var trader = await _traderRepository.SetBalance(id, balance);
            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<CloudStockDbModel> GetTraderMine(int id, int mineId)
        {
            var trader = await _traderRepository.GetTrader(id);
            var traderMine = trader.CloudStock.FirstOrDefault(cloudStock => cloudStock.MineId == id);
            return traderMine;
        }

        public async Task<List<CloudStockDbModel>> AddTraderMine(int id, AddTraderMineModel mine)
        {
            var trader = await _traderRepository.AddTraderMine(id, mine.MineId);
            return trader;
        }

        public TraderResponseModel MapFromDbToTraderResponseModel(TraderDbModel dbModel)
        {
            return _mapper.Map<TraderResponseModel>(dbModel);
        }


    }
}
