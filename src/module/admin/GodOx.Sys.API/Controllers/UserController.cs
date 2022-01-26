using AutoMapper;
using GodOx.Share.Caches;
using GodOx.Share.Repository;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Enums;
using GodOx.Sys.API.Enums.Extension;
using GodOx.Sys.API.Jwt;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Dtos.Output;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using GodOx.Sys.API.Attributes;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{/// <summary>
 /// 用户控制器
 /// </summary>
    public class UserController : ApiControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IBaseServer<User> _userService;
        private readonly IBaseServer<R_User_Role> _r_User_RoleService;
        private readonly IMenuService _menuService;
        private readonly ICacheHelper _cacheHelper;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;
        public const string loginRSACrypt = "loginRSACrypt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtHelper"></param>
        /// <param name="userService"></param>
        /// <param name="r_User_RoleService"></param>
        /// <param name="menuService"></param>
        /// <param name="cacheHelper"></param>
        /// <param name="currentUserContext"></param>
        /// <param name="mapper"></param>
        public UserController(JwtHelper jwtHelper, IBaseServer<User> userService, IBaseServer<R_User_Role> r_User_RoleService, IMenuService menuService, ICacheHelper cacheHelper, ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _jwtHelper = jwtHelper;
            _userService = userService;
            _r_User_RoleService = r_User_RoleService;
            _menuService = menuService;
            _cacheHelper = cacheHelper;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult RemoveMenuCache(int userId)
        {
            //由于使用了匿名访问，必须前台传值用户id进来，后台拿不到用户当前的值。
            //IMenuService:LoadLeftMenuTreesAsync:[1]  清理左侧树形菜单缓存
            _cacheHelper.Remove($"IMenuService:LoadLeftMenuTreesAsync:[{userId}]");
            return Ok(new { code = 1, msg = "服务端成功清理左侧树形菜单缓存" });
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userRegisterInput"></param>
        /// <returns></returns>
        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] UserRegisterInput userRegisterInput)
        {
            userRegisterInput.Password = Md5Crypt.Encrypt(userRegisterInput.Password);
            var userModel = _mapper.Map<User>(userRegisterInput);
            userModel.CreateTime = DateTime.Now;
            userModel.Ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
            userModel.Address = IpParseHelper.GetAddressByIP(userModel.Ip);
            var i = await _userService.AddAsync(userModel);
            return i > 0 ? new ApiResult(i) : new ApiResult("用户添加失败！");
        }

        [HttpPost, Authority]
        public async Task<ApiResult> Modify([FromBody] UserModifyInput userModifyInput)
        {
            var model = await _userService.GetModelAsync(d => d.Id == userModifyInput.Id);
            if (!model.Password.Equals(userModifyInput.Password))
            {
                userModifyInput.Password = Md5Crypt.Encrypt(userModifyInput.Password);
            }
            var i = await _userService.UpdateAsync(d => new User()
            {
                Name = userModifyInput.Name,
                Password = userModifyInput.Password,
                Mobile = userModifyInput.Mobile,
                Status = userModifyInput.Status,
                Remark = userModifyInput.Remark,
                Email = userModifyInput.Email,
                TrueName = userModifyInput.TrueName,
                Sex = userModifyInput.Sex,
                ModifyTime = DateTime.Now
            },
            d => d.Id == userModifyInput.Id);

            return i > 0 ? new ApiResult(i) : new ApiResult("用户修改失败！");
        }

        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput input)
        {
            return new ApiResult(await _userService.DeleteAsync(input.Ids));
        }
        [HttpPost]
        public async Task<ApiResult> ModfiyPwd([FromBody] ModifyPwdInput modifyPwdInput)
        {
            if (modifyPwdInput.Id == 0)
            {
                modifyPwdInput.Id = _currentUserContext.Id;
            }
            if (!modifyPwdInput.ConfirmPassword.Equals(modifyPwdInput.NewPassword))
            {
                throw new ArgumentNullException("两次输入的密码不一致");
            }
            modifyPwdInput.OldPassword = Md5Crypt.Encrypt(modifyPwdInput.OldPassword);
            var model = await _userService.GetModelAsync(d => d.Id == modifyPwdInput.Id);
            if (model.Id <= 0)
            {
                throw new ArgumentNullException("用户信息为空");
            }
            if (model.Password == modifyPwdInput.OldPassword)
            {
                throw new ArgumentNullException("旧密码错误!");
            }
            modifyPwdInput.ConfirmPassword = Md5Crypt.Encrypt(modifyPwdInput.ConfirmPassword);
            var i = await _userService.UpdateAsync(d => new User() { Password = modifyPwdInput.ConfirmPassword }, d => d.Id == modifyPwdInput.Id);
            return i > 0 ? new ApiResult(i) : new ApiResult("用户密码修改失败！");
        }
        [HttpGet]
        public async Task<ApiResult> GetUser(int id)
        {
            var model = await _userService.GetModelAsync(d => d.Id == id);
            var data = _mapper.Map<UserOutput>(model);
            return new ApiResult(data);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authority(Action = nameof(Button.Auth))]
        public async Task<ApiResult> GetListPages(int page, string key)
        {
            Expression<Func<User, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Name.Contains(key);
            }
            var res = await _userService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 设置角色（授权）
        /// </summary>
        /// <returns></returns>
        [HttpPost, Authority(Action = nameof(Button.Auth))]
        public async Task<ApiResult> SetRole([FromBody] SetUserRoleInput input)
        {
            //分配角色
            int i = 0;
            if (input.Status)
            {
                var model = await _r_User_RoleService.GetModelAsync(d => d.UserId == input.UserId && d.RoleId == input.RoleId && d.Status);
                if (model.Id > 0)
                {
                    return new ApiResult("已经存在该角色了", 500);
                }
                R_User_Role addModel = new R_User_Role() { UserId = input.UserId, RoleId = input.RoleId, CreateTime = DateTime.Now, Status = true };
                i = await _r_User_RoleService.AddAsync(addModel);
                return i > 0 ? new ApiResult(i) : new ApiResult("用户角色关联失败！");
            }
            else
            {
                i = await _r_User_RoleService.UpdateAsync(d => new R_User_Role() { Status = false }, d => d.UserId == input.UserId && d.RoleId == input.RoleId);
                // await DeleteAsync(d => d.UserId == input.UserId && d.RoleId == input.RoleId);
                //删除的话 要把授权的权限都要删除掉 风险比较高。
                return i > 0 ? new ApiResult(i) : new ApiResult("用户角色状态设置失败！");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ApiResult LoadLoginInfo()
        {
            var rsaKey = RSACrypt.GetKey();
            var number = Guid.NewGuid().ToString();
            if (rsaKey.Count <= 0 || rsaKey == null)
            {
                throw new ArgumentNullException("获取登录的公钥和私钥为空");
            }
            //获得公钥和私钥
            _cacheHelper.Set(SysCacheKey.loginRSACrypt + number, rsaKey);
            return new ApiResult(data: new { RsaKey = rsaKey, Number = number });
        }
        /// <summary>
        ///用户前后端分离的登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("v1/sign-in")]
        [AllowAnonymous]
        public async Task<ApiResult<LoginOutput>> SignIn([FromBody] LoginInput loginInput)
        {
            var rsaKey = _cacheHelper.Get<List<string>>(SysCacheKey.loginRSACrypt + loginInput.NumberGuid);
            if (rsaKey == null)
            {
                return new ApiResult<LoginOutput>("登录失败，请刷新浏览器再次登录!");
            }
            //Ras解密密码
            var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
            loginInput.Password = ras.Decrypt(loginInput.Password);
            var result = await LoginAsync(loginInput);
            var token = _jwtHelper.GetJwtToken(result.Data);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResult<LoginOutput>("生成的token字符串为空!");
            }
            result.Data.Token = token;
            return result;
        }
        /// <summary>
        /// mvc非前后端分离登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ApiResult<LoginOutput>> MvcLogin([FromBody] LoginInput loginInput)
        {
            try
            {
                var rsaKey = _cacheHelper.Get<List<string>>($"{SysCacheKey.LoginKey}:{loginInput?.NumberGuid}");
                if (rsaKey == null)
                {
                    throw new ArgumentException("登录失败，请刷新浏览器再次登录!");
                }
                var ras = new RSACrypt(rsaKey[0], rsaKey[1]);
                loginInput.Password = ras.Decrypt(loginInput.Password);
                var result = await LoginAsync(loginInput);
                if (result.StatusCode == 500 || result.StatusCode == 200 && result.Success == false)
                {
                    result.Data = new LoginOutput();
                    return result;
                }
                //请求当前用户的所有权限并存到缓存里面并发给前端 准备后面鉴权使用
                var menuAuths = await _menuService.GetCurrentAuthMenus(result.Data.Id);
                if (menuAuths == null || menuAuths.Count == 0)
                {
                    return new ApiResult<LoginOutput>("不好意思，该用户当前没有权限。请联系系统管理员分配权限！");
                }
                result.Data.MenuAuthOutputs = menuAuths;
                var identity = new ClaimsPrincipal(
                   new ClaimsIdentity(new[]
                       {
                              new Claim(JwtRegisteredClaimNames.Sid,result.Data.Id.ToString()),
                              new Claim(ClaimTypes.Name,result.Data.LoginName),
                              new Claim(ClaimTypes.WindowsAccountName,result.Data.LoginName),
                              new Claim(ClaimTypes.UserData,result.Data.LoginTime.ToString()),
                              new Claim(ClaimTypes.Email,result.Data.Mobile),
                              new Claim("trueName",result.Data.TrueName)
                       }, CookieAuthenticationDefaults.AuthenticationScheme)
                  );
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(24),
                    IsPersistent = true,
                    AllowRefresh = false
                });
                _cacheHelper.Remove($"{SysCacheKey.LoginKey}:{loginInput.NumberGuid}");
                new LogHelper().Process(loginInput.LoginName, LogEnum.Login.GetEnumText(), $"登陆成功", LogLevel.Info);
                return result;
            }
            catch (Exception ex)
            {
                ApiResult<LoginOutput> result = new ApiResult<LoginOutput>(msg: $"登陆失败，请重新刷新浏览器登录！{ex.Message}");
                new LogHelper().Process(loginInput.LoginName, LogEnum.Login.GetEnumText(), $"登陆失败:{ex.Message}", LogLevel.Error, ex);
                return result;
            }
        }

        private async Task<ApiResult<LoginOutput>> LoginAsync(LoginInput loginInput)
        {
            loginInput.Password = Md5Crypt.Encrypt(loginInput.Password);
            var loginModel = await _userService.GetModelAsync(d => d.Name.Equals(loginInput.LoginName) && d.Password.Equals(loginInput.Password));
            if (loginModel?.Id == 0)
            {
                new LogHelper().Process(loginModel?.Name, LogEnum.Login.GetEnumText(), $"{loginModel?.Name}登陆失败，用户名或密码错误！", LogLevel.Info);
                return new ApiResult<LoginOutput>("用户名或密码错误", 500);
            }
            if (!loginInput.ConfirmLogin)
            {
                if (loginModel.IsLogin)
                {
                    return new ApiResult<LoginOutput>($"该用户[{loginInput.LoginName}]已经登录，此时强行登录，其他地方会被挤下线！", 200);
                }
            }
            string ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();
            string address = IpParseHelper.GetAddressByIP(ip);
            var lastLoginTime = DateTime.Now;
            await _userService.UpdateAsync(d => new User()
            {
                LastLoginTime = lastLoginTime,
                Ip = ip,
                Address = address,
                IsLogin = true
            }, d => d.Id == loginModel.Id);
            var data = _mapper.Map<LoginOutput>(loginModel);
            return new ApiResult<LoginOutput>(data);
        }


    }
}
