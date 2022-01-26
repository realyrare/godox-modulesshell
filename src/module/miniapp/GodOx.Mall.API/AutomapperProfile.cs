using AutoMapper;
using GodOx.Mall.API.Models.Dtos.Output;
using GodOx.Mall.API.Models.Entity;

namespace GodOx.Mall.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //shop
            CreateMap<Goods, GoodsDetailOutput>();
        }
    }
}
