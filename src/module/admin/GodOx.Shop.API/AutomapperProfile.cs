using AutoMapper;
using GodOx.Shop.API.Models.Dtos.Input;
using GodOx.Shop.API.Models.Entity;

namespace GodOx.Shop.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //shop
            CreateMap<GoodsInput, Goods>();
            CreateMap<GoodsModifyInput, Goods>();
            CreateMap<Goods, GoodsModifyInput>();

            CreateMap<GoodsSpec, GoodsSpecInput>();


            CreateMap<CategoryInput, Category>();
        }
    }
}
