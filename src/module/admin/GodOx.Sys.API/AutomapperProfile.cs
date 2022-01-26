using AutoMapper;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Dtos.Output;
using GodOx.Sys.API.Models.Entity;

namespace GodOx.Sys.API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //sys
            CreateMap<User, LoginOutput>().ForMember(d => d.LoginName, s => s.MapFrom(i => i.Name));
            CreateMap<User, UserOutput>();
            CreateMap<UserRegisterInput, User>();

            CreateMap<RoleInput, Role>();
            CreateMap<RoleModifyInput, Role>();

            CreateMap<MenuModifyInput, Menu>();
            CreateMap<MenuInput, Menu>();
            //ParentMenuOutput
            CreateMap<Menu, ParentMenuOutput>();
            CreateMap<Menu, MenuAuthOutput>();

            CreateMap<ConfigInput, Config>();
            CreateMap<TenantInput, Tenant>();
            CreateMap<TenantModifyInput, Tenant>();
        }
    }
}
