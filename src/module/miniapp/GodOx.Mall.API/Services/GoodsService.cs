using AutoMapper;
using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Enums.Extension;
using GodOx.Auth.API.Models.Dtos.Common;
using GodOx.Mall.API.Enums;
using GodOx.Mall.API.Models.Dtos.Output;
using GodOx.Mall.API.Models.Entity;
using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
using SqlSugar;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：CategoryService
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/10 11:26:55
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Services
{
    public interface IGoodsService : IBaseServer<Goods>
    {
        Task<ApiResult> DetailAsync(int id);
        /// <summary>
        /// 小程序首页
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<ApiResult> GetByWherePageAsync(ListTenantQuery query, Expression<Func<Goods, Category, GoodsSpec, object>> orderBywhere, OrderByType sort, Expression<Func<Goods, Category, GoodsSpec, bool>> where = null);
        /// <summary>
        /// 立即购买
        /// </summary>
        /// <returns></returns>
        Task<ApiResult> GetBuyNowAsync(int goodsId, int goodsNum, string goodsNo, int tenantId);
        /// <summary>
        /// 商品信息判断（添加购物车，下单共用）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="specSkuId"></param>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<Tuple<Goods, GoodsSpec>> GoodInfoIsExist(int goodsId, int goodsNum, string specSkuId, int appUserId);
    }
    public class GoodsService : BaseServer<Goods>, IGoodsService
    {
        private readonly IMapper _mapper;

        public GoodsService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ApiResult> DetailAsync(int id)
        {
            Goods goods = await GetModelAsync(d => d.Id == id && d.Status);
            if (goods == null) throw new ArgumentNullException($"此商品{id}没有查找对应的商品信息");
            var model = _mapper.Map<GoodsDetailOutput>(goods);
            if (model.SpecType == SpecTypeEnum.Single.GetValue<int>())
            {
                var goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.GoodsId == id).FirstAsync();
                model.GoodsSpecOutput = _mapper.Map<GoodsSpecOutput>(goodsSpec);
            }
            return new ApiResult(model);
        }
        public async Task<Tuple<Goods, GoodsSpec>> GoodInfoIsExist(int goodsId, int goodsNum, string specSkuId, int appUserId)
        {
            Goods goodsModel = await GetModelAsync(d => d.Id == goodsId && d.Status);
            if (goodsModel?.Id == null)
            {
                throw new ArgumentNullException($"此商品{goodsId}没有查找到对应的商品信息");
            }
            if (goodsModel.GoodsStatus == GoodsStatusEnum.SoldOut.GetValue<int>())
            {
                throw new ArgumentNullException($"此商品已经下架");
            }
            GoodsSpec goodsSpec = null;
            if (!string.IsNullOrEmpty(specSkuId))
            {
                //多规格
                // var skuIds = specSkuId.Split('_');
                // List<int> ids = new List<int>();
                // for (int i = 0; i < skuIds.Length; i++)
                // {
                //     ids.Add(Convert.ToInt32(skuIds[i]));
                // }
                // //根据skuid查出对应的商品id
                //var goodsIds =await Db.Queryable<GoodsSpecRel>().Where(d => d.Status && ids.Contains(d.SpecValueId)).Select(d=>d.GoodsId).ToListAsync();
                // if (goodsIds.Count>0)
                // {
                //     var goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && goodsIds.Contains(d.GoodsId)).FirstAsync();

                // }
                goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && d.GoodsId == goodsId && d.SpecSkuId.Equals(specSkuId)).FirstAsync();
            }
            else
            {
                goodsSpec = await Db.Queryable<GoodsSpec>().Where(d => d.Status && d.GoodsId == goodsId).FirstAsync();
            }

            if (!(goodsSpec.StockNum > 0 & goodsSpec.StockNum > goodsNum))
            {
                throw new ArgumentNullException($"商品库存不存，目前仅剩{goodsSpec.StockNum}件");
            }
            return new Tuple<Goods, GoodsSpec>(goodsModel, goodsSpec);
        }

        public async Task<ApiResult> GetBuyNowAsync(int goodsId, int goodsNum, string goodsNo, int tenantId)
        {
            var model = await Db.Queryable<Goods, GoodsSpec>((g, gc) => new JoinQueryInfos(JoinType.Inner, g.Id == gc.GoodsId && gc.GoodsNo == goodsNo))
                  .Where((g, gc) => g.TenantId == tenantId && g.Id == goodsId && gc.Id == goodsId)
                  .Select((g, gc) => new
                  {
                      g.Id,
                      g.ImgUrl,
                      gc.GoodsNo,
                      gc.GoodsPrice,
                      gc.StockNum
                  }).FirstAsync();

            if (model?.Id == null)
            {
                return new ApiResult("未找到该商品");
            }
            if (!(model.StockNum > 0 & model.StockNum > goodsNum))
            {
                throw new ArgumentNullException($"商品库存不存，目前仅剩{model.StockNum}件");
            }
            var totalPrice = model.GoodsPrice * goodsNum;
            return new ApiResult(new
            {
                GoodsList = model, //单价*数量 
                OrderTotalPrice = totalPrice
            });

        }
        public async Task<ApiResult> GetByWherePageAsync(ListTenantQuery query, Expression<Func<Goods, Category, GoodsSpec, object>> orderBywhere, OrderByType sort, Expression<Func<Goods, Category, GoodsSpec, bool>> where = null)
        {
            var datas = await Db.Queryable<Goods, Category, GoodsSpec>((g, c, gc) => new JoinQueryInfos(
                JoinType.Inner, g.CategoryId == c.Id && g.TenantId == query.TenantId, JoinType.Inner, g.Id == gc.GoodsId))
                .Where((g, c, gc) => g.Status && g.GoodsStatus == 10)
                .WhereIF(where != null, where)
              .OrderBy(orderBywhere, OrderByType.Desc)
              .Select((g, c, gc) => new GoodsOutput()
              {
                  Name = g.Name,
                  CategoryName = c.Name,
                  SalesActual = g.SalesActual,
                  Id = g.Id,
                  ImgUrl = g.ImgUrl,
                  GoodsPrice = gc.GoodsPrice,
                  GoodsSales = gc.GoodsSales,
                  LinePrice = gc.LinePrice,
              }).ToPageAsync(query.Page, query.Limit);
            foreach (var item in datas.Items)
            {
                item.ImgUrl = !string.IsNullOrEmpty(item.ImgUrl) ? item.ImgUrl.Split(',')[0] : "";
            }
            return new ApiResult(datas);
        }
    }
}