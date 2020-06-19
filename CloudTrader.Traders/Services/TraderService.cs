﻿using System.Threading.Tasks;
using CloudTrader.Traders.Data;
using CloudTrader.Traders.Service;

namespace CloudTrader.Traders.Services
{
    public interface ITraderService
    {
        Task<Trader> Create(Trader trader);

        Task<Trader> GetById(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly ITraderRepository _traderRepository;

        public TraderService(ITraderRepository traderRepository)
        {
            _traderRepository = traderRepository;
        }

        public async Task<Trader> Create(Trader trader)
        {
            await _traderRepository.SaveTrader(trader);

            return trader;
        }

        public async Task<Trader> GetById(int id)
        {
            return await _traderRepository.GetTrader(id);
        }        
    }
}
