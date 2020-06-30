using System;

namespace CloudTrader.Traders.Service.Exceptions
{
    public class TraderNotFoundException : Exception
    {
        public TraderNotFoundException(int id)
            : base($"Trader with id \"{id}\" not found")
        {
        }
    }
}
