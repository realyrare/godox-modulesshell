using AutoMapper;
using GodOx.Mvc.Admin.Common;
using GodOx.Share.Caches;
using GodOx.Share.Repository;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Enums;
using GodOx.Sys.API.Enums.Extension;
using GodOx.Sys.API.Models.Dtos.Output;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public partial class UserController : Controller
    {

        private readonly ICacheHelper _cacheHelper;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IBaseServer<User> _service;
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;
        public UserController(ICacheHelper cacheHelper, ICurrentUserContext currentUserContext, IBaseServer<User> service, IMenuService menuService, IMapper mapper)
        {
            _cacheHelper = cacheHelper;
            _currentUserContext = currentUserContext;
            _service = service;
            _menuService = menuService;
            _mapper = mapper;
        }
        /// <summary>
        /// 用户首页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SetRole(int id)
        {
            ViewBag.UserId = id;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            User model = null;
            if (id == 0)
            {
                model = new User();
            }
            else
            {
                model = await _service.GetModelAsync(d => d.Id == id && d.Status);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ModifyPwd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CurrentUserInfo()
        {
            UserOutput userOutput = new UserOutput();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userOutput.Name = HttpContext.User.Identity.Name;
                userOutput.Id = Convert.ToInt32(HttpContext.User.Claims.Where(d => d.Type == JwtRegisteredClaimNames.Sid).Select(d => d.Value).FirstOrDefault());
                userOutput.Mobile = HttpContext.User.Claims.Where(d => d.Type == "mobile").Select(d => d.Value).FirstOrDefault();
                userOutput.Email = HttpContext.User.Claims.Where(d => d.Type == ClaimTypes.Email).Select(d => d.Value).FirstOrDefault();
                userOutput.TrueName = HttpContext.User.Claims.Where(d => d.Type == "trueName").Select(d => d.Value).FirstOrDefault();
            }
            else
            {
                Redirect("/user/login");
            }
            return View(userOutput);
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new ArgumentNullException("获取登录的公钥和私钥为空");
            }
            ViewBag.RsaKey = rsaKey[0];
            ViewBag.Number = number;
            //获得公钥和私钥
            _cacheHelper.Set($"{SysCacheKey.LoginKey}:{number}", rsaKey);
            return View();
        }

        [HttpGet, AllowAnonymous]
        public FileResult OnGetVCode()
        {
            var vcode = VerifyCode.CreateRandomCode(4);
            HttpContext.Session.SetString("vcode", vcode);
            var img = VerifyCode.DrawImage(vcode, 20, Color.White);
            return File(img, "image/gif");
        }

        [HttpGet]
        public async Task<ApiResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _cacheHelper.Remove($"{SysCacheKey.AuthMenu}:{_currentUserContext.Id}");
            try
            {
                new LogHelper().Process(_currentUserContext.Name, LogEnum.Login.GetEnumText(), $"{_currentUserContext.Name}成功退出系统!", LogLevel.Info);
                //设置用户退出
                await _service.UpdateAsync(d => new User() { IsLogin = false }, d => d.Id == _currentUserContext.Id);
                return new ApiResult();
            }
            catch
            {
                return new ApiResult("退出失败了！");
            }
        }
    }
}
