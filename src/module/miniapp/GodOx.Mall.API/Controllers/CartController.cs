using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Auth.API.Enums.Extension;
using GodOx.Mall.API.Enums;
using GodOx.Mall.API.Models.Entity;
using GodOx.Mall.API.Services;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using GodOx.Auth.API.Models.Dtos.Output;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;
using GodOx.Mall.API.Models.Dtos.Output;

namespace GodOx.Mall.API.Controllers
{/// <summary>
/// 购物车
/// </summary>
    public class CartController : AppBaseController
    {
        private readonly IBaseServer<Cart> _cartService;
        private readonly DbContext _dbContext;
        private readonly IGoodsService _goodService;

        public CartController(IBaseServer<Cart> cartService, DbContext dbContext, IGoodsService goodService)
        {
            _cartService = cartService;
            _dbContext = dbContext;
            _goodService = goodService;
        }
        /// <summary>
        /// 购物车删除
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Delete([FromForm] int goodsId)
        {
            var i = await _cartService.UpdateAsync(d => new Cart() { Status = false }, d => d.GoodsId == goodsId && d.AppUserId == HttpWx.AppUserId);
            return Result(i);
        }
        [HttpPost]
        public async Task<ApiResult> Add([FromForm] int goodsId, [FromForm] int goodsNum, [FromForm] string specSkuId)
        {
            var goodsData = await _goodService.GoodInfoIsExist(goodsId, goodsNum, specSkuId, HttpWx.AppUserId);
            var isExistCartModel = await _cartService.GetModelAsync(l => l.AppUserId == HttpWx.AppUserId && l.GoodsId == goodsId);
            int i = 0;
            if (isExistCartModel?.Id > 0)
            {
                goodsNum += isExistCartModel.GoodsNum;
                i = await _cartService.UpdateAsync(d => new Cart() { GoodsNum = goodsNum, SpecSkuId = specSkuId, ModifyTime = DateTime.Now }, d => d.Id == isExistCartModel.Id && d.AppUserId == HttpWx.AppUserId);
            }
            else
            {
                Cart model = new Cart()
                {
                    GoodsId = goodsData.Item1.Id,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    AppUserId = HttpWx.AppUserId,
                    GoodsNum = goodsNum,
                    SpecSkuId = specSkuId
                };
                i = await _cartService.AddAsync(model);
            }
            return Result(i);
        }
        /// <summary>
        /// 减掉商品数量
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Sub([FromForm] int goodsId)
        {
            var isExistCartModel = await _cartService.GetModelAsync(l => l.AppUserId == HttpWx.AppUserId && l.GoodsId == goodsId);

            if (isExistCartModel?.Id != null)
            {
                if (isExistCartModel.GoodsNum < 1)
                {
                    return new ApiResult("该商品在购物车已经不存在了");
                }
                isExistCartModel.GoodsNum -= 1;
                await _cartService.UpdateAsync(d => new Cart() { GoodsNum = isExistCartModel.GoodsNum }, d => d.Id == isExistCartModel.Id && d.AppUserId == HttpWx.AppUserId);
            }
            return new ApiResult(msg: "删减成功", 200);
        }
        [HttpGet]
        public async Task<ApiResult> Lists(int tenantId)
        {
            var cartList = await _cartService.GetListAsync(l => l.AppUserId == HttpWx.AppUserId);
            var inCludeGoods = cartList.Select(d => d.GoodsId).ToList();
            var goodsList = await _dbContext.Db.Queryable<Goods, GoodsSpec>((g, gc) => new JoinQueryInfos(JoinType.Inner, g.Id == gc.GoodsId))
               .Where((g, gc) => g.TenantId == tenantId)
               .Select((g, gc) => new CartGoodsOutput
               {
                   GoodsId = g.Id,
                   ImgUrl = g.ImgUrl,
                   GoodsPrice = gc.GoodsPrice,
                   SpecType = g.SpecType,
                   LinePrice = gc.LinePrice,
                   GoodsSales = gc.GoodsSales,
                   SalesActual = g.SalesActual,
                   SpecMany = g.SpecMany
               }).ToListAsync();

            double totalPrice = 0;
            foreach (var item in goodsList)
            {
                //去查商品对应的购物车中的数量
                var cartGoodsNum = cartList.Where(d => d.GoodsId == item.GoodsId).Select(d => d.GoodsNum).FirstOrDefault();
                item.OrderTotalNum = cartGoodsNum;
                totalPrice += (double)item.GoodsPrice * cartGoodsNum;
                if (item.SpecType == SpecTypeEnum.Multi.GetValue<int>())
                {
                    //显示规格组和值

                }
            }
            return new ApiResult(new
            {
                GoodsList = goodsList, //单价*数量 
                OrderTotalPrice = totalPrice
            });
        }

    }
}
