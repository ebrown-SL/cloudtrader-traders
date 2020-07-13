using AutoMapper;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Models.Service;

namespace CloudTrader.Traders.Service
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
