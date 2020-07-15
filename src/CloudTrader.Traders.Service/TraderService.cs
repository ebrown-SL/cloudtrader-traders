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
        Task<GetTraderMinesResponseModel> GetTraderMines(int id);
        Task<TraderResponseModel> SetBalance(int id, int balance);
        Task<CloudStockDetail> GetTraderMine(int id, int mineId);
        Task<GetTraderMinesResponseModel> SetTraderMine(int id, SetTraderMineModel mine);
        Task<GetTraderMinesResponseModel> DeleteTraderMine(int id, int mineId);
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

        public async Task<GetTraderMinesResponseModel> GetTraderMines(int id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            return _mapper.Map<GetTraderMinesResponseModel>(trader);
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

        public async Task<CloudStockDetail> GetTraderMine(int id, int mineId)
        {
            var trader = await _traderRepository.GetTrader(id);
            var traderMine = trader.CloudStockDbModels.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine == null)
            {
                throw new MineNotFoundException(mineId, id);
            }
            return _mapper.Map<CloudStockDetail>(traderMine);
        }

        public async Task<GetTraderMinesResponseModel> SetTraderMine(int id, SetTraderMineModel mine)
        {
            var trader = await _traderRepository.SetTraderMine(id, mine.MineId, mine.Stock);
            return _mapper.Map<GetTraderMinesResponseModel>(trader);
        }
        public async Task<GetTraderMinesResponseModel> DeleteTraderMine(int id, int mineId)
        {
            var trader = await _traderRepository.DeleteTraderMine(id, mineId);
            return _mapper.Map<GetTraderMinesResponseModel>(trader);
        }

        public TraderResponseModel MapFromDbToTraderResponseModel(TraderDbModel dbModel)
        {
            return _mapper.Map<TraderResponseModel>(dbModel);
        }
    }
}
