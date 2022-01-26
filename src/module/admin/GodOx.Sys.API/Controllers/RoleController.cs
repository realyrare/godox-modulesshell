using AutoMapper;
using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{
    public class RoleController : ApiControllerBase
    {
        private readonly IBaseServer<Role> _roleService;
        private readonly IBaseServer<R_User_Role> _r_User_roleService;
        private readonly IMapper _mapper;
        private readonly IBaseServer<R_Role_Menu> _r_Role_MenuService;

        public RoleController(IBaseServer<Role> roleService, IMapper mapper, IBaseServer<R_Role_Menu> r_Role_MenuService, IBaseServer<R_User_Role> r_User_roleService)
        {
            _roleService = roleService;
            _mapper = mapper;
            _r_Role_MenuService = r_Role_MenuService;
            _r_User_roleService = r_User_roleService;
        }
        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _roleService.DeleteAsync(commonDeleteInput.Ids));
        }
        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Role, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Name.Contains(key);
            }
            var res = await _roleService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 根据角色获取当前已授权或未授权的所有角色
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="userId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetListPagesByUser(int userId, int page = 1, int limit = 15)
        {
            var query = await _roleService.GetPagesAsync(page, limit);
            var userRoleList = await _r_User_roleService.GetListAsync(d => d.UserId == userId && d.Status);
            foreach (var item in query.Items)
            {
                var model = userRoleList.FirstOrDefault(d => d.RoleId == item.Id);
                if (model != null)
                {
                    item.Status = true;
                }
                else
                {
                    item.Status = false;
                }
            }
            return new ApiResult(data: new { count = query.TotalItems, items = query.Items });
        }
        [HttpGet, Authority]
        public async Task<ApiResult> Detail(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }
            var res = await _roleService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpGet]
        public async Task<ApiResult> List()
        {
            var data = await _roleService.GetListAsync();
            return new ApiResult(data: data);
        }

        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] RoleInput roleInput)
        {
            var role = _mapper.Map<Role>(roleInput);
            return new ApiResult(await _roleService.AddAsync(role));
        }

        [HttpPost, Authority(Action = nameof(Button.Auth))]
        public async Task<ApiResult> SetMenu(SetRoleMenuInput setRoleMenuInput)
        {
            var allUserMenus = await _r_Role_MenuService.GetListAsync(d => d.IsPass);
            // allUserRoles.Where(d => d.UserId == setUserRoleInput.UserId && setUserRoleInput.RoleIds.Contains(d.RoleId));
            List<R_Role_Menu> list = new List<R_Role_Menu>();
            foreach (var item in setRoleMenuInput.MenuIds)
            {
                var model = allUserMenus.Where(d => d.RoleId == setRoleMenuInput.RoleId && d.MenuId == item);
                if (model == null)
                {
                    var r_User_Menu = new R_Role_Menu() { RoleId = setRoleMenuInput.RoleId, MenuId = item, IsPass = true, CreateTime = DateTime.Now };
                    list.Add(r_User_Menu);
                    //add                    
                }
            }
            var i = await _r_Role_MenuService.AddListAsync(list);
            return i > 0 ? new ApiResult() : new ApiResult("设置菜单失败了！");
        }

        [HttpPut, Authority]
        public async Task<ApiResult> Modify([FromBody] RoleModifyInput roleModifyInput)
        {
            return new ApiResult(await _roleService.UpdateAsync(d => new Role()
            {
                Name = roleModifyInput.Name,
                Description = roleModifyInput.Description,
                ModifyTime = roleModifyInput.ModifyTime
            }, d => d.Id == roleModifyInput.Id));
        }
    }
}
