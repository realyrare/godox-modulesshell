/*************************************
* 类名：TenantController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:22:57
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/
using AutoMapper;
using GodOx.Share.Caches;
using GodOx.Share.FileManage;
using GodOx.Share.Repository;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{
    [Route("api/sys/[controller]/[action]")]
    public class TenantController : ApiBaseController<Tenant, DetailQuery, DeletesInput, KeyListQuery, TenantInput, TenantModifyInput>
    {
        private readonly IBaseServer<Tenant> _service;
        private readonly ICacheHelper _cacheHelper;
        private readonly IUploadFile _uploadHelper;
        private readonly ICurrentUserContext _currentUserContext;
        public TenantController(IBaseServer<Tenant> service, IMapper mapper, ICacheHelper cacheHelper, IUploadFile uploadHelper, ICurrentUserContext currentUserContext) : base(service, mapper)
        {
            _service = service;
            _cacheHelper = cacheHelper;
            _uploadHelper = uploadHelper;
            _currentUserContext = currentUserContext;
        }
        [HttpGet, Authority]
        public override async Task<ApiResult> Detail([FromQuery] DetailQuery detailQuery)
        {
            var res = await _service.GetModelAsync(d => d.Id == detailQuery.Id && d.IsDel == false);
            return new ApiResult(data: res);
        }
        [HttpDelete, Authority]
        public override async Task<ApiResult> Deletes([FromBody] DeletesInput deletesInput)
        {
            foreach (var item in deletesInput.Ids)
            {
                await _service.UpdateAsync(d => new Tenant() { IsDel = false }, d => d.Id == item);
            }
            return new ApiResult();
        }
        /// <summary>
        /// 设置当前站点
        /// </summary>
        /// <param name="TenantCurrentInput"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> SetCurrent([FromBody] TenantCurrentInput TenantCurrentInput)
        {
            //把之前缓存存储的站点拿出来设置为不是当前的。
            var model = await _service.GetModelAsync(d => d.Id == TenantCurrentInput.Id && d.IsDel == false && d.IsCurrent == false);
            if (model == null)
            {
                throw new ArgumentNullException("当前站点实体信息为空!");
            }
            var currentTenantId = _cacheHelper.Get<int>($"{SysCacheKey.CurrentTenant}:{_currentUserContext.Id}");
            if (currentTenantId != 0)
            {
                await _service.UpdateAsync(d => new Tenant() { IsCurrent = false }, d => d.Id == currentTenantId);
            }

            model.IsCurrent = true;
            await _service.UpdateAsync(model);
            //这里最好更新下缓存
            _cacheHelper.Set($"{SysCacheKey.CurrentTenant}:{_currentUserContext.Id}", model);
            return new ApiResult();
        }
        [HttpGet]
        public async Task<ApiResult> GetList()
        {
            //首页加载该列表时赋值于缓存
            var res = await _service.GetListAsync(d => d.IsDel == false);
            foreach (var item in res)
            {
                if (item.IsCurrent)
                {
                    _cacheHelper.Set($"{SysCacheKey.CurrentTenant}:{_currentUserContext.Id}", item.Id);
                }
            }
            return new ApiResult(data: res);
        }
        [HttpGet, Authority]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListQuery keyListQuery)
        {
            Expression<Func<Tenant, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(keyListQuery.Key))
            {
                whereExpression = d => d.Title.Contains(keyListQuery.Key);
            }
            var res = await _service.GetPagesAsync(keyListQuery.Page, keyListQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpPost, AllowAnonymous]
        public ApiResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "tenant/");
            return new ApiResult(data: data);
        }
    }
}