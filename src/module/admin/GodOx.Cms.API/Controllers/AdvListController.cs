using AutoMapper;
using GodOx.Cms.API.Models.Dtos.Input;
using GodOx.Cms.API.Models.Entity;
using GodOx.Share.FileManage;
using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Threading.Tasks;

/*************************************
* 类 名： AdvListController
* 作 者： realyrare
* 邮 箱： mahonggang8888@126.com
* 时 间： 2021/3/16 18:09:15
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Controllers
{
    [Route("api/cms/[controller]/[action]")]
    public class AdvListController : ApiTenantBaseController<AdvList, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, AdvListInput, AdvListModifyInput>
    {

        private readonly IUploadFile _uploadHelper;
        private readonly DbContext _dbContext;

        public AdvListController(IBaseServer<AdvList> service, IMapper mapper, IUploadFile uploadHelper, DbContext dbContext) : base(service, mapper)
        {
            _uploadHelper = uploadHelper;
            _dbContext = dbContext;
        }
        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var res = await _dbContext.Db.Queryable<AdvList>().Where(d => d.Status && d.TenantId == query.TenantId)
               .WhereIF(!string.IsNullOrEmpty(query.Key), d => d.Title.Contains(query.Key))
                  .OrderBy(c => c.CreateTime, OrderByType.Desc)
               .Select(
               d => new AdvList()
               {
                   TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == d.TenantId).Select(s => s.Name),
                   Id = d.Id,
                   CreateTime = d.CreateTime,
                   Type = d.Type,
                   ModifyTime = d.ModifyTime,
                   Status = d.Status,
                   Summary = d.Summary,
                   Title = d.Title
               }
               ).ToPageAsync(query.Page, query.Limit);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpPost, AllowAnonymous]
        public ApiResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "advList/");
            return new ApiResult(data: data);
        }
    }
}