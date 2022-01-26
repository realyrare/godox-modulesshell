using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Mall.API.Models.Dtos.Input;
using GodOx.Mall.API.Models.Entity;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

/*************************************
* 类名：AppUserController
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/23 11:01:15
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Controllers
{
    /// <summary>
    /// 小程序用户地址
    /// </summary>
    public class AppUserAddressController : AppBaseController
    {

        private readonly IBaseServer<AppUserAddress> _appUserAddressService;
        private readonly DbContext _dbContext;
        public AppUserAddressController(IBaseServer<AppUserAddress> appUserAddressService, DbContext dbContext)
        {
            _appUserAddressService = appUserAddressService;
            _dbContext = dbContext;
        }

        private Tuple<string, string, string> GetNamesBySplitRegions(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                throw new ArgumentNullException("用户省市区为空!");
            }
            var regionsArry = regionName.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return new Tuple<string, string, string>(regionsArry[0], regionsArry[1], regionsArry[2]);
        }
        [HttpGet]
        public async Task<ApiResult> Lists()
        {
            var addressList = await _appUserAddressService.GetListAsync(d => d.Status == false && d.AppUserId == HttpWx.AppUserId);
            return new ApiResult(addressList);
        }
        [HttpGet]
        public async Task<ApiResult> Detail(int addressId)
        {
            var addressModel = await _appUserAddressService.GetModelAsync(d => d.Status == false && d.AppUserId == HttpWx.AppUserId && d.Id.Equals(addressId));
            return new ApiResult(addressModel);
        }
        [HttpPost]
        public async Task<ApiResult> Add([FromForm] AddressInput input)
        {
            var tupleValue = GetNamesBySplitRegions(input.Region);
            AppUserAddress model = new AppUserAddress()
            {

                CreateTime = DateTime.Now,
                AppUserId = HttpWx.AppUserId,
                Status = false,
                Name = input.Name,
                Detail = input.Detail,
                Phone = input.Phone,
                Province = tupleValue.Item1,
                City = tupleValue.Item2,
                Region = tupleValue.Item3,
            };
            var sign = await _appUserAddressService.AddAsync(model);
            return Result(sign);
        }
        [HttpPost]
        public async Task<ApiResult> Edit([FromForm] AddressInput input)
        {
            var tupleValue = GetNamesBySplitRegions(input.Region);
            var sign = await _appUserAddressService.UpdateAsync(d => new AppUserAddress()
            {
                Name = input.Name,
                Detail = input.Detail,
                Province = tupleValue.Item1,
                City = tupleValue.Item2,
                Region = tupleValue.Item3,
                Phone = input.Phone,
                ModifyTime = DateTime.Now
            }, d => d.Status == false && d.Id == input.AddressId && d.AppUserId == HttpWx.AppUserId);
            return Result(sign);
        }

        [HttpPost]
        public async Task<ApiResult> Delete([FromForm] int addressId)
        {
            var sign = await _appUserAddressService.UpdateAsync(d => new AppUserAddress() { Status = false }, d => d.Id == addressId);
            return new ApiResult(sign);
        }

        [HttpPost]
        public async Task<ApiResult> SetIsDefault([FromForm] int addressId)
        {
            //把有默认地址的取消
            try
            {
                _dbContext.Db.BeginTran();
                var result1 = await _dbContext.Db.Updateable<AppUserAddress>().SetColumns(d => new AppUserAddress() { IsDefault = false }).Where(d => d.Status && d.AppUserId == HttpWx.AppUserId).ExecuteCommandAsync();

                //设置当前新的默认地址
                var result2 = await _dbContext.Db.Updateable<AppUserAddress>().SetColumns(d => new AppUserAddress() { IsDefault = false }).Where(d => d.Status && d.AppUserId == HttpWx.AppUserId && d.Id == addressId).ExecuteCommandAsync();
                if (result1 > 0 && result2 > 0)
                {
                    _dbContext.Db.CommitTran();
                }
            }
            catch
            {
                _dbContext.Db.RollbackTran();
                return new ApiResult("地址设置失败！");
            }
            return new ApiResult();
        }
    }
}