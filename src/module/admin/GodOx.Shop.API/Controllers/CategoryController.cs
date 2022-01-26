using AutoMapper;
using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
using GodOx.Shop.API.Models.Dtos.Input;
using GodOx.Shop.API.Models.Entity;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Entity;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodOx.Shop.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    [Route("api/shop/[controller]/[action]")]
    [MultiTenant]
    public class CategoryController : ApiControllerBase
    {
        private readonly IBaseServer<Category> _service;
        private readonly DbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContex;

        public CategoryController(IBaseServer<Category> service, DbContext dbcontext, IMapper mapper, ICurrentUserContext currentUserContex)
        {
            _service = service;
            _dbcontext = dbcontext;
            _mapper = mapper;
            _currentUserContex = currentUserContex;
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var res = await _dbcontext.Db.Queryable<Category>().Where(d => d.Status && d.TenantId == query.TenantId)
                .WhereIF(!string.IsNullOrEmpty(query.Key), c => c.Name.Contains(query.Key))
                .OrderBy(c => c.CreateTime, OrderByType.Desc)
                .Select(c => new Category()
                {
                    Id = c.Id,
                    TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == c.TenantId).Select(s => s.Name),
                    Name = c.Name,
                    CreateTime = c.CreateTime,
                    Layer = c.Layer,
                    ModifyTime = c.ModifyTime,
                    IconSrc = c.IconSrc
                })
                .ToPageAsync(query.Page, 2000);
            var result = new List<Category>();
            if (!string.IsNullOrEmpty(query.Key))
            {
                var menuModel = await _service.GetModelAsync(m => m.Name.Contains(query.Key));
                WebHelper.ChildNode(res.Items, result, menuModel.ParentId);
            }
            else
            {
                WebHelper.ChildNode(res.Items, result, 0);
            }
            if (result?.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
                }
                return new ApiResult(data: new { count = res.TotalItems, items = result });
            }
            else
            {
                return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
            }
        }
        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] CategoryInput input)
        {
            var CategoryModel = await _service.GetModelAsync(d => d.Name.Equals(input.Name));
            if (CategoryModel?.Id > 0)
            {
                throw new ArgumentNullException("已经存在类目名称了");
            }
            var category = _mapper.Map<Category>(input);
            var categoryId = await _service.AddAsync(category);
            var result = await WebHelper.DealTreeData(input.ParentId, categoryId, async () =>
           await _service.GetModelAsync(d => d.Id == input.ParentId));
            var i = await _service.UpdateAsync(d => new Category() { ParentList = result.Item2, Layer = result.Item1 }, d => d.Id == categoryId);
            return new ApiResult(i);
        }
        [HttpPut, Authority]
        public async Task<ApiResult> Modify([FromBody] CategoryModifyInput input)
        {
            var categoryModel = await _service.GetModelAsync(d => d.Name.Equals(input.Name) && d.Id != input.Id);
            if (categoryModel?.Id > 0)
            {
                throw new ArgumentNullException("已经存在类目名称了");
            }
            var result = await WebHelper.DealTreeData(input.ParentId, input.Id, async () =>
              await _service.GetModelAsync(d => d.Id == input.ParentId));
            var i = await _service.UpdateAsync(d => new Category()
            {
                Name = input.Name,
                IconSrc = input.IconSrc,
                Layer = result.Item1,
                ParentList = result.Item2,
                ModifyTime = DateTime.Now,
                TenantId = input.TenantId,
                ParentId = input.ParentId
            }, d => d.Id == input.Id);
            return new ApiResult(i);
        }
        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAllParentCategory()
        {
            var list = await _service.GetListAsync(d => d.Status && d.TenantId == _currentUserContex.TenantId);
            var data = new List<Category>();
            WebHelper.ChildNode(list, data, 0);
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Name = WebHelper.LevelName(item.Name, item.Layer);
                }
            }
            return new ApiResult(data);
        }
    }
}
