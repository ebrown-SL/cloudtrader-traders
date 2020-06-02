using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudtrader_traders.Models
{
    public class Trader
    {
        public Trader() {}
        public Trader(AuthenticationModel model)
        {
            Id = 0;
            Username = model.Username;
            Balance = 100;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Balance { get; set; }
        public Trader getTraderPasswordRedacted()
        {
            /*this.PasswordHash = null;
            this.PasswordSalt = null;*/
            return this;
        }
    }
}
