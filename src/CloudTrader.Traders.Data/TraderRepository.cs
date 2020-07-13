using System.Threading.Tasks;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Service;

namespace CloudTrader.Traders.Data
{
    public class TraderRepository : ITraderRepository
    {
        private readonly TraderContext _context;

        public TraderRepository(TraderContext context)
        {
            _context = context;
        }

        public async Task<TraderDbModel> GetTrader(int id)
        {
            var trader = await _context.Traders.FindAsync(id);
            return trader;
        }

        public async Task<TraderDbModel> SaveTrader(TraderDbModel trader)
        {
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();
            return trader;
        }
    }
}
