using GodOx.Auth.API.Common;
using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Models.Dtos.Input;
using GodOx.Auth.API.Models.Dtos.Output;
using GodOx.Auth.API.Models.Entity;
using GodOx.Share.Caches;
using GodOx.Share.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
/*************************************
* 类名：AppUserController
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/27 16:37:29
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/
namespace GodOx.Auth.API.Controllers
{
    [Route("api/app/[controller]/[action]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IBaseServer<AppUser> _appUserService;
        private readonly ICacheHelper _cacheHelper;
        private readonly HttpHelper _httpHelper;


        public AppUserController(IBaseServer<AppUser> appUserService, ICacheHelper cacheHelper, HttpHelper httpHelper)
        {
            _appUserService = appUserService;
            _cacheHelper = cacheHelper;
            _httpHelper = httpHelper;
        }

        [HttpPost]
        public async Task<ApiResult> Login([FromForm] AppUserLoginInput input)
        {
            var result = new ApiResult();
            try
            {
                // 微信登录 获取session_key
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ArgumentNullException($"{nameof(input.Code)} code为空");
                }
                //微信登录，根据code获得openid
                //var wxres = await WxTools.GetOpenId(input.Code);
                var url = $"https://api.weixin.qq.com/sns/jscode2session?appid={AppConfig.AppId}&secret={AppConfig.AppSecret}&js_code={input.Code}&grant_type=authorization_code";
                var res = await _httpHelper.GetAsync<HttpWxOutput>(url);
                if (res == null)
                {
                    throw new ArgumentNullException("获取用户实体信息为空");
                }
                if (!string.IsNullOrEmpty(res.Errmsg))
                {
                    result.StatusCode = res.Errcode;
                    result.Msg = res.Errmsg;
                    return result;
                }
                // 自动注册用户
                var userId = await WxAppLoginAndReg(res.Openid, input.UserInfo, input.TenantId);
                // 生成token (session3rd)
                var token = WxToken(input.TenantId, res.Openid);
                // 记录缓存, 7天
                HttpWxUserOutput wxUserOutput = (HttpWxUserOutput)res;
                wxUserOutput.AppUserId = userId;
                _cacheHelper.Set(token, wxUserOutput, TimeSpan.FromDays(7));
                result.Data = new { userId, token };
                return result;
            }
            catch (Exception e)
            {
                result.StatusCode = 500;
                result.Success = false;
                result.Msg = e.Message;
                return result;
            }
        }



        private async Task<int> WxAppLoginAndReg(string openId, string userInfo, int tenantId)
        {
            var requestUser = JsonConvert.DeserializeObject<AppUserInput>(userInfo.Replace("\\", ""));
            //先判断是否存在
            var dbRes = await _appUserService.GetModelAsync(m => m.OpenId == openId);
            if (dbRes?.Id != null)
            {
                dbRes.ModifyTime = DateTime.Now;
            }
            else
            {
                dbRes.CreateTime = DateTime.Now;
            }
            dbRes.NickName = requestUser.NickName;
            dbRes.Gender = requestUser.Gender;
            dbRes.AvatarUrl = requestUser.AvatarUrl;
            dbRes.Country = requestUser.Country;
            dbRes.Province = requestUser.Province;
            dbRes.City = requestUser.City;
            dbRes.OpenId = openId;
            dbRes.TenantId = tenantId;
            if (dbRes?.Id != null)
            {
                await _appUserService.UpdateAsync(dbRes);
                return dbRes.Id;
            }
            return await _appUserService.AddAsync(dbRes);

        }
        private string WxToken(int tenantId, string openId)
        {
            return Md5Crypt.Encrypt($"{tenantId}_{DateTime.Now}_{openId}_{Guid.NewGuid()}_EncryptKey");
        }
    }
}