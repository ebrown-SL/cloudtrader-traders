using AutoMapper;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Models.Service;

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
