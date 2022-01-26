/*************************************
* 类名：IndexController
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/30 16:48:29
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Auth.API.Models.Dtos.Common;
using GodOx.Mall.API.Services;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Threading.Tasks;

namespace GodOx.Mall.API.Controllers
{
    /// <summary>
    /// 小程序首页
    /// </summary>
    public class IndexController : AppBaseController
    {
        private readonly IGoodsService _goodsService;
        //private readonly IAdvListService _advListService;

        public IndexController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
            //  _advListService = advListService;
        }
        [HttpGet]
        public async Task<ApiResult> Page()
        {
            var query = new ListTenantQuery() { Page = 1, Limit = 4, TenantId = HttpWx.TenantId };
            var newest = await _goodsService.GetByWherePageAsync(query, (g, c, gc) => g.Id, OrderByType.Desc);
            query.Limit = 10;
            var best = await _goodsService.GetByWherePageAsync(query, (g, c, gc) => gc.GoodsSales, OrderByType.Desc);
            // var items = await _advListService.GetListAsync(d=>d.Type==AdvEnum.MiniApp);
            return new ApiResult(new { newest, best });
        }
    }
}