using System.Collections.Generic;
using System.Threading.Tasks;
using CloudtraderTraders.Helpers;
using CloudtraderTraders.Models;

namespace CloudtraderTraders.Services
{
    public interface ITraderService
    {
        Task<TraderModel> Create(TraderModel trader);

        Task<IEnumerable<TraderModel>> GetAll();

        Task<TraderModel> GetById(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly DataContext _context;

        public TraderService(DataContext context)
        {
            _context = context;
        }

        public async Task<TraderModel> Create(TraderModel trader)
        {
            _context.Users.Add(trader);
            await _context.SaveChangesAsync();

            return trader;
        }

        public async Task<IEnumerable<TraderModel>> GetAll()
        {
            return await Task.Run(() => _context.Users);
        }

        public async Task<TraderModel> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }        
    }
}
