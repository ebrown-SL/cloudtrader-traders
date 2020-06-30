using System;

namespace CloudTrader.Traders.Service.Exceptions
{
    public class TraderAlreadyExistsException : Exception
    {
        public TraderAlreadyExistsException(int id)
            : base($"Trader with id \"{id}\" already exists")
        {
        }
    }
}
