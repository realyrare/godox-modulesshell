using AutoMapper;
using GodOx.Cms.API.Models.Dtos.Input;
using GodOx.Cms.API.Models.Entity;
using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
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

/*************************************
* 类名：ColumnController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:24:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Controllers
{
    [Route("api/cms/[controller]/[action]")]
    public class ColumnController : ApiTenantBaseController<Column, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, ColumnInput, ColumnModifyInput>
    {
        private readonly IBaseServer<Column> _service;
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;
        private readonly ICurrentUserContext _currentUserContext;

        public ColumnController(IBaseServer<Column> service, IMapper mapper, DbContext dbContext, ICurrentUserContext currentUserContext) : base(service, mapper)
        {
            _service = service;
            _mapper = mapper;
            _dbContext = dbContext;
            _currentUserContext = currentUserContext;
        }

        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var res = await _dbContext.Db.Queryable<Column>().Where(d => d.Status && d.TenantId == query.TenantId)
               .WhereIF(!string.IsNullOrEmpty(query.Key), c => c.Title.Contains(query.Key))
               .OrderBy(c => c.CreateTime, OrderByType.Desc)
               .Select(c => new Column()
               {
                   Id = c.Id,
                   TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == c.TenantId).Select(s => s.Name),
                   Title = c.Title,
                   CreateTime = c.CreateTime,
                   EnTitle = c.EnTitle,
                   Layer = c.Layer,
                   ModifyTime = c.ModifyTime
               })
               .ToPageAsync(query.Page, query.Limit);

            var result = new List<Column>();
            if (!string.IsNullOrEmpty(query.Key))
            {
                var menuModel = await _service.GetModelAsync(m => m.Title.Contains(query.Key));
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
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
                return new ApiResult(data: new { count = res.TotalItems, items = result });
            }
            else
            {
                return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
            }
        }
        [HttpPost, Authority]
        public override async Task<ApiResult> Add([FromBody] ColumnInput columnInput)
        {
            var columnModel = await _service.GetModelAsync(d => d.Title.Equals(columnInput.Title));
            if (columnModel?.Id > 0)
            {
                throw new ArgumentNullException("已经存在类目名称了");
            }
            var column = _mapper.Map<Column>(columnInput);
            var columnId = await _service.AddAsync(column);
            var result = await WebHelper.DealTreeData(columnInput.ParentId, columnId, async () =>
           await _service.GetModelAsync(d => d.Id == columnInput.ParentId));
            var i = await _service.UpdateAsync(d => new Column() { ParentList = result.Item2, Layer = result.Item1 }, d => d.Id == columnId);
            return new ApiResult(i);
        }
        [HttpPut, Authority]
        public override async Task<ApiResult> Modify([FromBody] ColumnModifyInput columnModifyInput)
        {
            var columnModel = await _service.GetModelAsync(d => d.Title.Equals(columnModifyInput.Title) && d.Id != columnModifyInput.Id);
            if (columnModel?.Id > 0)
            {
                throw new ArgumentNullException("已经存在类目名称了");
            }
            var result = await WebHelper.DealTreeData(columnModifyInput.ParentId, columnModifyInput.Id, async () =>
              await _service.GetModelAsync(d => d.Id == columnModifyInput.ParentId));

            var i = await _service.UpdateAsync(d => new Column()
            {
                Title = columnModifyInput.Title,
                EnTitle = columnModifyInput.EnTitle,
                Attr = columnModifyInput.Attr,
                SubTitle = columnModifyInput.SubTitle,
                Summary = columnModifyInput.Summary,
                ImgUrl = columnModifyInput.ImgUrl,
                Layer = result.Item1,
                ParentList = result.Item2,
                ModifyTime = DateTime.Now,
                Keyword = columnModifyInput.Keyword,
                TenantId = columnModifyInput.TenantId,
                ParentId = columnModifyInput.ParentId
            }, d => d.Id == columnModifyInput.Id);
            return new ApiResult(i);
        }
        /// <summary>
        /// 所有父栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAllParentColumn()
        {
            var list = await _service.GetListAsync(d => d.Status && d.TenantId == _currentUserContext.TenantId);
            var data = new List<Column>();
            WebHelper.ChildNode(list, data, 0);
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Title = WebHelper.LevelName(item.Title, item.Layer);
                }
            }
            return new ApiResult(data);
        }
    }
}