using CloudTrader.Traders.Domain.Models;
using CloudTrader.Traders.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service
{
    public class TraderService : ITraderService
    {
        private readonly ITraderRepository traderRepository;

        public TraderService(ITraderRepository traderRepository)
        {
            this.traderRepository = traderRepository;
        }

        public async Task<Trader> CreateTrader(int initialBalance)
        {
            var newTrader = new Trader { Balance = initialBalance };

            var savedTrader = await traderRepository.CreateTrader(newTrader);
            return savedTrader;
        }

        public async Task<Trader> GetTrader(Guid id)
        {
            var trader = await traderRepository.GetTrader(id);
            if (trader == null)
            {
                throw new TraderNotFoundException(id);
            }

            return trader;
        }

        public async Task<ICollection<CloudStock>> GetTraderMines(Guid traderId)
        {
            var trader = await traderRepository.GetTrader(traderId);
            if (trader == null)
            {
                throw new TraderNotFoundException(traderId);
            }
            return trader.CloudStocks;
        }

        public async Task<List<Trader>> GetTraders()
        {
            var traders = await traderRepository.GetTraders();
            return traders;
        }

        public async Task<IEnumerable<TraderCloudStock>> GetTradersByMineId(Guid mineId)
        {
            var traders = await traderRepository.GetTradersByMineId(mineId);
            var mappedTraders = traders
                .Select(t => new TraderCloudStock(t, mineId));

            return mappedTraders;
        }

        public async Task<Trader> SetBalance(Guid traderId, int newBalance)
        {
            var trader = await traderRepository.GetTrader(traderId);

            if (trader == null)
                throw new TraderNotFoundException(traderId);

            trader.Balance = newBalance;

            trader = await traderRepository.UpdateTrader(trader);

            return trader;
        }

        public async Task<Trader> UpdateBalance(Guid traderId, int amountToAdd)
        {
            var trader = await traderRepository.GetTrader(traderId);

            if (trader == null)
                throw new TraderNotFoundException(traderId);

            var newBalance = trader.Balance + amountToAdd;
            trader.Balance = newBalance;

            trader = await traderRepository.UpdateTrader(trader);

            return trader;
        }

        public async Task<CloudStock> GetTraderMine(Guid traderId, Guid mineId)
        {
            var trader = await traderRepository.GetTrader(traderId);
            if (trader == null)
            {
                throw new TraderNotFoundException(traderId);
            }
            var traderMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine == null)
            {
                throw new MineNotFoundException(mineId, traderId);
            }
            return traderMine;
        }

        public async Task<ICollection<CloudStock>> SetTraderMine(Guid traderId, Guid mineId, int newStock)
        {
            var trader = await traderRepository.GetTrader(traderId);
            if (trader == null)
                throw new TraderNotFoundException(traderId);

            trader.CloudStocks ??= new HashSet<CloudStock>();

            var existingMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);

            if (existingMine == null)
            {
                trader.CloudStocks.Add(new CloudStock { MineId = mineId, Stock = newStock });
            }
            else
            {
                existingMine.Stock = newStock;
            }

            trader = await traderRepository.UpdateTrader(trader);

            return trader.CloudStocks;
        }

        public async Task<Trader> DeleteTraderMine(Guid traderId, Guid mineId)
        {
            var trader = await traderRepository.GetTrader(traderId);
            if (trader == null)
                throw new TraderNotFoundException(traderId);

            var traderMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine != null)
            {
                trader.CloudStocks.Remove(traderMine);
            }

            trader = await traderRepository.UpdateTrader(trader);

            return trader;
        }
    }
}