using cloudtrader_traders.Models;
using System.Collections.Generic;
using System.Linq;

namespace cloudtrader_traders.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Trader> WithoutPasswords(this IEnumerable<Trader> users)
        {
            return users.Select(x => x.getTraderPasswordRedacted());
        }
    }
}