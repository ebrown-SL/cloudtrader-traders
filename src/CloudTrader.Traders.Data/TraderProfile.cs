using AutoMapper;
using CloudTrader.Traders.Service;

namespace CloudTrader.Traders.Data
{
    public class TraderProfile : Profile
    {
        public TraderProfile()
        {
            CreateMap<Trader, TraderDbModel>()
                .ReverseMap();
        }
    }
}
