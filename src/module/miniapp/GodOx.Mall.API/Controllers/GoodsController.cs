using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Auth.API.Models.Dtos.Common;
using GodOx.Mall.API.Models.Entity;
using GodOx.Mall.API.Services;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
/*************************************
* 类名：GoodsController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/31 14:46:34
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Controllers
{/// <summary>
/// 商品管理
/// </summary>
    public class GoodsController : AppBaseController
    {
        private readonly IGoodsService _goodsService;

        public GoodsController(IGoodsService goodsService)
        {
            _goodsService = goodsService;
        }
        [HttpGet]
        public Task<ApiResult> BuyNow(int goodsId, int goodsNum, string goodsNo)
        {
            /*订单购买  查询商品需要知道商品的id,商品的编码*/
            //查询实体
            return _goodsService.GetBuyNowAsync(goodsId, goodsNum, goodsNo, HttpWx.TenantId);
        }
        [HttpGet]
        public Task<ApiResult> Detail(int goodsId)
        {
            return _goodsService.DetailAsync(goodsId);
        }
        [HttpGet]
        public Task<ApiResult> Lists(string sortType, int sortPrice, int categoryId)
        {
            var query = new ListTenantQuery() { Page = 1, Limit = 20, TenantId = HttpWx.TenantId };
            Expression<Func<Goods, Category, GoodsSpec, bool>> expression = (g, c, gc) => g.Status == true;

            Expression<Func<Goods, Category, GoodsSpec, object>> order = (g, c, gc) => g.CreateTime;

            OrderByType sort = OrderByType.Desc;

            if (categoryId > 0)
            {
                expression = (g, c, gc) => g.CategoryId == categoryId;
            }

            if (sortType == "all" && sortPrice == 0)
            {
                order = (g, c, gc) => g.SalesActual;
            }
            else if (sortType == "all" && sortPrice == 1)
            {
                order = (g, c, gc) => g.Id;
            }
            else if (sortType == "sales" && sortPrice == 1)
            {
                order = (g, c, gc) => gc.GoodsSales;
            }
            else if (sortType == "price" && sortPrice == 0)
            {
                order = (g, c, gc) => gc.GoodsPrice;
                sort = OrderByType.Asc;
            }
            else if (sortType == "price" && sortPrice == 1)
            {
                order = (g, c, gc) => gc.GoodsPrice;
            }

            return _goodsService.GetByWherePageAsync(query, order, sort, expression);

        }
    }
}