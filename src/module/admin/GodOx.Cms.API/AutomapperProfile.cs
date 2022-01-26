using AutoMapper;
using GodOx.Cms.API.Models.Dtos.Input;
using GodOx.Cms.API.Models.Entity;

namespace GodOx.Cms.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {

            //cms

            CreateMap<ColumnInput, Column>();
            CreateMap<ColumnModifyInput, Column>();
            CreateMap<ArticleInput, Article>();
            CreateMap<ArticleModifyInput, Article>();

            CreateMap<AdvListInput, AdvList>();
            CreateMap<AdvListModifyInput, AdvList>();

            CreateMap<KeywordInput, Keyword>();
            CreateMap<KeywordModifyInput, Keyword>();




        }
    }
}
