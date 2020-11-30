﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudTrader.Traders.Models.Api.Request;
using CloudTrader.Traders.Models.Api.Response;
using CloudTrader.Traders.Models.POCO;
using CloudTrader.Traders.Service.Exceptions;

namespace CloudTrader.Traders.Service
{
    public interface ITraderService
    {
        Task<TraderResponseModel> CreateTrader(CreateTraderRequestModel balance);
        Task<TraderResponseModel> GetTrader(Guid id);
        Task<GetAllTradersResponseModel> GetTraders();
        Task<GetTradersByMineIdResponseModel> GetTradersByMineId(Guid mineId);
        Task<GetTraderMinesResponseModel> GetTraderMines(Guid id);
        Task<TraderResponseModel> SetBalance(Guid id, SetTraderBalanceRequestModel balance);
        Task<TraderResponseModel> UpdateBalance(Guid id, UpdateTraderBalanceRequestModel amount);
        Task<CloudStockResponseModel> GetTraderMine(Guid id, Guid mineId);
        Task<GetTraderMinesResponseModel> SetTraderMine(Guid id, SetTraderMineRequestModel mine);
        Task<GetTraderMinesResponseModel> DeleteTraderMine(Guid id, Guid mineId);
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

        public async Task<TraderResponseModel> CreateTrader(CreateTraderRequestModel balance)
        {
            balance ??= new CreateTraderRequestModel();
            var trader = await _traderRepository.SaveTrader(new Trader { Balance = balance.Balance });

            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<TraderResponseModel> GetTrader(Guid id)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }

            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<GetTraderMinesResponseModel> GetTraderMines(Guid id)
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

        public async Task<GetTradersByMineIdResponseModel> GetTradersByMineId(Guid mineId)
        {
            var traders = await _traderRepository.GetTradersByMineId(mineId);
            var mappedTraders = _mapper.Map<List<TraderCloudStockResponseModel>>(traders);
            return new GetTradersByMineIdResponseModel(mappedTraders);
        }

        public async Task<TraderResponseModel> SetBalance(Guid id, SetTraderBalanceRequestModel balance)
        {
            var trader = await _traderRepository.SetBalance(id, balance.Balance);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<TraderResponseModel> UpdateBalance(Guid id, UpdateTraderBalanceRequestModel amount)
        {
            var trader = await _traderRepository.UpdateBalance(id, amount.Amount);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            return MapFromDbToTraderResponseModel(trader);
        }

        public async Task<CloudStockResponseModel> GetTraderMine(Guid id, Guid mineId)
        {
            var trader = await _traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            var traderMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine == null)
            {
                throw new MineNotFoundException(mineId, id);
            }
            return _mapper.Map<CloudStockResponseModel>(traderMine);
        }

        public async Task<GetTraderMinesResponseModel> SetTraderMine(Guid id, SetTraderMineRequestModel mine)
        {
            var trader = await _traderRepository.SetTraderMine(id, mine.MineId, mine.Stock);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }
            return _mapper.Map<GetTraderMinesResponseModel>(trader);
        }

        public async Task<GetTraderMinesResponseModel> DeleteTraderMine(Guid id, Guid mineId)
        {
            var trader = await _traderRepository.DeleteTraderMine(id, mineId);
            return _mapper.Map<GetTraderMinesResponseModel>(trader);
        }

        public TraderResponseModel MapFromDbToTraderResponseModel(Trader dbModel)
        {
            return _mapper.Map<TraderResponseModel>(dbModel);
        }
    }
}
