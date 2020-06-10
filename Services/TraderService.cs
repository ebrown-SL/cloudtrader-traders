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
        Task<Trader> Authenticate(string username, string password);
        Task<Trader> Create(Trader user, string password);
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

        public async Task<Trader> Authenticate(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            // return null if user not found
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful so return user details without password
            return user.GetTraderPasswordRedacted();
        }

        public async Task<IEnumerable<Trader>> GetAll()
        {
            return await Task.Run(() => _context.Users.WithoutPasswords());
        }

        public async Task<Trader> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Trader> Create(Trader user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                throw new Exception("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Id = _context.Users.Count() + 1;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        /*
        public static void logout(string name, string password)
        {

        }*/

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
