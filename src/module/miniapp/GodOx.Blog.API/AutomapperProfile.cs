using AutoMapper;
using GodOx.Blog.API.Models.Dtos.Input;
using GodOx.Blog.API.Models.Entity;

namespace GodOx.Blog.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {

            CreateMap<MessageInput, Message>();
        }
    }
}
