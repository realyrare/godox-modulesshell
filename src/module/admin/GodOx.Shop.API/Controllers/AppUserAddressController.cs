using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
using GodOx.Shop.API.Models.Entity;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using GodOx.Shop.API.Models.Dtos.Output;
using GodOx.Sys.API.Attributes;
using SqlSugar;
using System.Threading.Tasks;

/*************************************
* 类名：AppUserController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/23 11:01:15
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Shop.API.Controllers
{
    [Route("api/shop/[controller]/[action]")]
    [MultiTenant]
    public class AppUserAddressController : ApiControllerBase
    {
        private readonly DbContext _dbContext;
        public AppUserAddressController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var data = await _dbContext.Db.Queryable<AppUserAddress, AppUser>((d, u) => new object[] { JoinType.Inner,d.Id==u.AddressId&&d.Status
            }).Where((d, u) => d.TenantId == query.TenantId).WhereIF(!string.IsNullOrEmpty(query.Key), (d, u) => d.Name.Equals(query.Key))
                .OrderBy((d, u) => d.Id, OrderByType.Desc)
                .Select((d, u) => new AppUserAddressOutput()
                {
                    Name = d.Name,
                    Province = d.Province,
                    City = d.City,
                    Detail = d.Detail,
                    Region = d.Region,
                    CreateTime = d.CreateTime,
                    AppUserName = u.NickName,
                    TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == d.TenantId).Select(s => s.Name),
                }).ToPageAsync(query.Page, query.Limit);
            return new ApiResult(data);
        }
    }
}