using GodOx.Shop.API.Models.Dtos.Query;
using GodOx.Shop.API.Services;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System.Threading.Tasks;

namespace GodOx.Shop.API.Controllers
{
    /// <summary>
    /// 商品订单控制器
    /// </summary>
    [Route("api/shop/[controller]/[action]")]
    [MultiTenant]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet, Authority]
        public Task<ApiResult> GetListPages([FromQuery] OrderKeyListTenantQuery query)
        {
            return _orderService.GetListPageAsync(query);
        }

    }
}
