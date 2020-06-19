using System.Threading.Tasks;
using AutoMapper;
using CloudTrader.Traders.Service;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public class TraderRepository : ITraderRepository
    {
        private readonly TraderContext _context;

        private readonly IMapper _mapper;

        public TraderRepository(IMapper mapper)
        {
            _mapper = mapper;

            var contextOptions = new DbContextOptionsBuilder<TraderContext>()
                .UseInMemoryDatabase(databaseName: "Traders")
                .Options;
            _context = new TraderContext(contextOptions);
        }

        public async Task<Trader> GetTrader(int id)
        {
            var traderDbModel = await _context.Traders.FindAsync(id);
            var trader = _mapper.Map<Trader>(traderDbModel);
            return trader;
        }

        public async Task SaveTrader(Trader trader)
        {
            var traderDbModel = _mapper.Map<TraderDbModel>(trader);
            _context.Traders.Add(traderDbModel);
            await _context.SaveChangesAsync();
        }
    }
}
