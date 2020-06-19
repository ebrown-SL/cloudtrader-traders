using System.Threading.Tasks;
using CloudTrader.Traders.Helpers;
using CloudTrader.Traders.Service.Models;

namespace CloudTrader.Traders.Repositories
{
    public interface ITraderRepository
    {
        Task SaveTrader(Trader trader);

        Task<Trader> GetTrader(int id);
    }

    public class TraderRepository : ITraderRepository
    {
        private readonly DataContext _context;

        public TraderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Trader> GetTrader(int id)
        {
            return await _context.Traders.FindAsync(id);
        }

        public async Task SaveTrader(Trader trader)
        {
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();
        }
    }
}
