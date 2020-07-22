using System;

namespace CloudTrader.Traders.Service.Exceptions
{
    public class MineNotFoundException : NotFoundException
    {
        public MineNotFoundException(int mineId, Guid traderId)
            : base($"Mine with id '{mineId}' not found for trader with id '{traderId}'") { }
    }
}
