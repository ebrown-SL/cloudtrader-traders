using System;

namespace CloudTrader.Traders.Service.Exceptions
{
    public class MineNotFoundException : Exception
    {
        public MineNotFoundException(int mineId, int traderId)
            : base($"Mine with id \"{mineId}\" not found for trader with id \"{traderId}\"") { }
    }
}
