using CloudtraderTraders.Models;
using System.Collections.Generic;
using System.Linq;

namespace CloudtraderTraders.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Trader> WithoutPasswords(this IEnumerable<Trader> users)
        {
            return users.Select(x => x.GetTraderPasswordRedacted());
        }
    }
}