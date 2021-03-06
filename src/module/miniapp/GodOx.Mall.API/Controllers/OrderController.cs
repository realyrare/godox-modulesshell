using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Auth.API.Enums.Extension;
using GodOx.Auth.API.Models.Entity;
using GodOx.Mall.API.Enums;
using GodOx.Mall.API.Models.Dtos.Output;
using GodOx.Mall.API.Models.Entity;
using GodOx.Mall.API.Services;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GodOx.Mall.API.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : AppBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderGoodsService _orderGoodsService;
        private readonly IBaseServer<AppUserAddress> _appUserAddressService;
        private readonly IBaseServer<AppUser> _appUserService;

        public OrderController(IOrderService orderService, IOrderGoodsService orderGoodsService, IBaseServer<AppUserAddress> appUserAddressService, IBaseServer<AppUser> appUserService)
        {
            _orderService = orderService;
            _orderGoodsService = orderGoodsService;
            _appUserAddressService = appUserAddressService;
            _appUserService = appUserService;
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult> Cancel([FromForm] int orderId, [FromForm] int goodsId)
        {
            return _orderGoodsService.CancelOrderAsync(orderId, goodsId, HttpWx.TenantId);
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult> Receipt([FromForm] int orderId)
        {
            return _orderGoodsService.ReceiptAsync(orderId, HttpWx.AppUserId);

        }

        /// <summary>
        /// 小程序订单列表
        /// </summary>
        /// <param name="dataType">订单类型</param>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult> GetList(string dataType)
        {
            return _orderGoodsService.GetListAsync(HttpWx.AppUserId, dataType);

        }
        [HttpGet]
        public Task<ApiResult<OrderDetailOutput>> Detail(int orderId)
        {
            return _orderService.GetOrderDetailAsync(orderId);
        }
        /// <summary>
        /// 立马就买
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult> BuyNow([FromForm] int goodsId, [FromForm] int goodsNum, [FromForm] string goodsSkuId)
        {
            return _orderGoodsService.BuyNowAsync(goodsId, goodsNum, goodsSkuId, HttpWx.AppUserId);
        }
        /// <summary>
        /// 购物车结算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResult> CartBuy()
        {
            return _orderGoodsService.CartBuyAsync(HttpWx.AppUserId);
        }

        /// <summary>
        /// 获取用户已经添加的收获地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetExistAddress()
        {
            //该收获地址 后面可能会做是否包邮判断           
            var addressModel = await _appUserAddressService.GetModelAsync(d => d.Status && d.AppUserId == HttpWx.AppUserId && d.IsDefault == true);
            if (addressModel == null)
            {
                addressModel = await _appUserAddressService.GetModelAsync(d => d.Status && d.AppUserId == HttpWx.AppUserId);
            }
            return new ApiResult(addressModel);
        }

        /// <summary>
        /// 用户中心订单统计
        /// </summary>
        /// <param name="wxappId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> Statistics()
        {
            var appUser = await _appUserService.GetModelAsync(d => d.Status && d.Id == HttpWx.AppUserId);
            var paymentCount = await GetOrderCount("payment");
            var receivedCount = await GetOrderCount("received");
            return new ApiResult(new { userInfo = appUser, orderCount = new { payment = paymentCount, received = receivedCount } });
        }

        private async Task<int> GetOrderCount(string type = "all")
        {
            Expression<Func<Order, bool>> where = null;
            switch (type)
            {
                case "payment":
                    where = l => l.PayStatus == PayStatusEnum.WaitForPay.GetValue<int>() && l.AppUserId == HttpWx.AppUserId;
                    break;
                case "received":
                    where = l => l.PayStatus == PayStatusEnum.Paid.GetValue<int>() && l.DeliveryStatus == DeliveryStatusEnum.Sended.GetValue<int>() && l.ReceiptStatus == ReceiptStatusEnum.WaitForReceiving.GetValue<int>() && l.AppUserId == HttpWx.AppUserId;
                    break;
                case "all":
                    break;
            }
            var result = await _orderService.CountAsync(where);
            return result;
        }
    }
}
