using System;

namespace CloudTrader.Traders.Service.Exceptions
{
    public class TraderNotFoundException : NotFoundException
    {
        public TraderNotFoundException(int id)
            : base($"Trader with id '{id}' not found") { }
    }
}
