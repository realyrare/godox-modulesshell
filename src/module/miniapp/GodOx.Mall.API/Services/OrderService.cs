using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Models.Entity;
using GodOx.Mall.API.Models.Dtos.Output;
using GodOx.Mall.API.Models.Entity;
using GodOx.Share.Repository;
using SqlSugar;
using System;
using System.Threading.Tasks;

/*************************************
* 类名：OrderService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/19 17:12:16
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Services
{
    public interface IOrderService : IBaseServer<Order>
    {

        Task<ApiResult<OrderDetailOutput>> GetOrderDetailAsync(int orderId);
    }
    public class OrderService : BaseServer<Order>, IOrderService
    {

        public async Task<ApiResult<OrderDetailOutput>> GetOrderDetailAsync(int orderId)
        {
            var model = await Db.Queryable<Order, OrderGoods, AppUser>((o, og, u) => new object[] {
                JoinType.Inner,o.Id==og.OrderId,
                JoinType.Inner,og.AppUserId==u.Id,
            })
              .Where((o, og, u) => o.Id == orderId && o.Status)
             .Select((o, og, u) => new OrderDetailOutput()
             {
                 OrderNo = o.OrderNo,
                 Id = o.Id,
                 AppUserName = u.NickName,
                 AppUserId = u.Id,
                 //  AppUserAddressId = u.AddressId,
                 OrderStatus = o.OrderStatus,
                 PayStatus = o.PayStatus,
                 DeliveryStatus = o.DeliveryStatus,
                 ReceiptStatus = o.ReceiptStatus,
                 TransactionId = o.TransactionId,
                 ExpressCompany = o.ExpressCompany,
                 ExpressPrice = o.ExpressPrice,
                 ExpressNo = o.ExpressNo,
                 DeliveryTime = o.DeliveryTime,
                 ReceiptTime = o.ReceiptTime,
                 TotalPrice = og.TotalPrice,
                 CreateTime = o.CreateTime,
                 TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == o.TenantId).Select(s => s.Name),

             }).FirstAsync();
            if (model == null)
            {
                throw new ArgumentNullException($"订单详情实体数据为空！");
            }
            //这里订单地址不使用id关联，防止用户地址表里面的地址更新后发生配送错误
            model.Address = await Db.Queryable<OrderAddress>().Where(oa => oa.AppUserId == model.AppUserId && oa.OrderId == model.Id).FirstAsync();

            model.GoodsDetailList = await Db.Queryable<OrderGoods>().Where(d => d.OrderId == orderId && d.Status).Select(d => new OrderGoodsDetailOutput
            {
                GoodsImg = d.ImgUrl,
                GoodsName = d.GoodsName,
                GoodsId = d.GoodsId,
                GoodsPrice = d.GoodsPrice,
                GoodsWeight = d.GoodsWeight,
                TotalNum = d.TotalNum,
                TotalPrice = d.TotalPrice,
                CreateTime = d.CreateTime,
                GoodsNo = d.GoodsNo
            }).ToListAsync();

            return new ApiResult<OrderDetailOutput>(model);
        }
    }


}