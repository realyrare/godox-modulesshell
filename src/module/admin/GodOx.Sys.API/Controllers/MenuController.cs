using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{
    public class MenuController : ApiControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IBaseServer<Config> _configService;
        private readonly IBaseServer<R_Role_Menu> _r_Role_MenuService;
        private readonly ICurrentUserContext _currentUserContext;

        public MenuController(IMenuService menuService, IBaseServer<Config> configService, IBaseServer<R_Role_Menu> r_Role_MenuService, ICurrentUserContext currentUserContext)
        {
            _menuService = menuService;
            _configService = configService;
            _r_Role_MenuService = r_Role_MenuService;
            _currentUserContext = currentUserContext;
        }
        [HttpGet]
        public async Task<ApiResult> GetBtnCodeList()
        {
            return new ApiResult(await _configService.GetListAsync(d => d.Type == nameof(Button)));
        }

        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            foreach (var item in commonDeleteInput.Ids)
            {
                await _menuService.UpdateAsync(d => new Menu() { Status = false }, d => d.Id == item);
            }
            return new ApiResult();
        }

        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            return await _menuService.GetListPagesAsync(page, key);
        }
        /// <summary>
        /// 获取菜单按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> BtnCode(int menuId = 0, int roleId = 0)
        {
            return await _menuService.BtnCodeByMenuIdAsync(menuId, roleId);
        }
        /// <summary>
        /// 树形菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> TreeByRole(int roleId)
        {
            return await _menuService.TreeRoleIdAsync(roleId);
        }
        /// <summary>
        /// 菜单按钮授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> SetBtnPermissions([FromBody] RoleMenuBtnInput input)
        {
            //根据角色和菜单查询内容
            var model = await _r_Role_MenuService.GetModelAsync(d => d.RoleId == input.RoleId && d.MenuId == input.MenuId);
            if (model.Id <= 0)
            {
                throw new ArgumentNullException("您还没有授权当前菜单功能模块");
            }
            if (model.BtnCodeIds != null)
            {
                //判断授权还是取消
                var list = model.BtnCodeIds.ToList();
                if (input.Status)
                {
                    //不包含则添加。包含放任不管
                    if (!list.Contains(input.BtnCodeId))
                    {
                        list.Add(input.BtnCodeId);
                    }
                }
                else
                {
                    //授权 包含则移除
                    if (list.Contains(input.BtnCodeId))
                    {
                        list.Remove(input.BtnCodeId);
                    }
                }
                model.BtnCodeIds = list.ToArray();
            }
            else
            {
                string[] arry = new string[] { input.BtnCodeId };
                //增加
                model.BtnCodeIds = arry;
            }
            var sign = await _r_Role_MenuService.UpdateAsync(d => new R_Role_Menu() { BtnCodeIds = model.BtnCodeIds, ModifyTime = DateTime.Now }, d => d.MenuId == input.MenuId && d.RoleId == input.RoleId);
            return sign > 0 ? new ApiResult(sign) : new ApiResult("菜单按钮授权失败！");


        }
        /// <summary>
        /// 授权菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Authority(Action = nameof(Button.Auth))]
        public async Task<ApiResult> AddPermissions([FromBody] PermissionsInput input)
        {
            var model = await _r_Role_MenuService.GetModelAsync(d => d.RoleId == input.RoleId && d.MenuId == input.MenuId);
            if (model.Id > 0)
            {
                return new ApiResult("已经存在该菜单权限了", 400);
            }
            R_Role_Menu addModel = new R_Role_Menu() { RoleId = input.RoleId, MenuId = input.MenuId, CreateTime = DateTime.Now };
            var sign = await _r_Role_MenuService.AddAsync(addModel);
            return new ApiResult(sign);
        }

        [HttpGet, Authority]
        public async Task<ApiResult> Detail(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var res = await _menuService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] MenuInput menuInput)
        {
            return await _menuService.AddToUpdateAsync(menuInput);
        }

        [HttpPut, Authority]
        public async Task<ApiResult> Modify([FromBody] MenuModifyInput menuModifyInput)
        {
            return await _menuService.ModifyAsync(menuModifyInput);
        }

        [HttpGet]
        public async Task<ApiResult> GetAllParentMenu()
        {
            return await _menuService.GetAllParentMenuAsync();
        }
        /// <summary>
        /// 左侧树形菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> LoadLeftMenuTrees()
        {

            var userId = _currentUserContext.Id;
            return await _menuService.LoadLeftMenuTreesAsync(userId);
        }
    }
}
