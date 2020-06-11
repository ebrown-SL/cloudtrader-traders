using CloudtraderTraders.Helpers;
using CloudtraderTraders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudtraderTraders.Services
{
    public interface ITraderService
    {
        Task<Trader> Create(Trader user);
        Task<IEnumerable<Trader>> GetAll();
        Task<Trader> GetById(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly DataContext _context;

        public TraderService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trader>> GetAll()
        {
            return await Task.Run(() => _context.Users);
        }

        public async Task<Trader> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Trader> Create(Trader user)
        {
            user.Id = _context.Users.Count() + 1;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
