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
* 类名：ArticleController
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/11 17:23:48
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Controllers
{
    [Route("api/cms/[controller]/[action]")]
    public class ArticleController : ApiTenantBaseController<Article, DetailTenantQuery, DeletesTenantInput, KeyListTenantQuery, ArticleInput, ArticleModifyInput>
    {
        private readonly IUploadFile _uploadHelper;
        private readonly DbContext _dbContext;
        public ArticleController(IBaseServer<Article> service, IMapper mapper, IUploadFile uploadHelper, DbContext dbContext) : base(service, mapper)
        {
            _uploadHelper = uploadHelper;
            _dbContext = dbContext;
        }
        [HttpGet]
        public override async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            var res = await _dbContext.Db.Queryable<Article, Column>((a, c) => new JoinQueryInfos(JoinType.Inner, a.ColumnId == c.Id && a.Status == true))
                 .WhereIF(query.TenantId != 0, (a, c) => a.TenantId == query.TenantId && c.TenantId == query.TenantId)
                .WhereIF(!string.IsNullOrEmpty(query.Key), (a, c) => a.Title.Contains(query.Key))
                   .OrderBy((a, c) => a.Id, OrderByType.Desc)
                   .Select((a, c) => new Article()
                   {
                       Title = a.Title,
                       CreateTime = a.CreateTime,
                       ModifyTime = a.ModifyTime,
                       Id = a.Id,
                       Audit = a.Audit,
                       Author = a.Author,
                       Source = a.Source,
                       ColumnName = c.Title,
                       TenantName = SqlFunc.Subqueryable<Tenant>().Where(s => s.Id == query.TenantId).Select(s => s.Name),
                   })
                   .ToPageAsync(query.Page, query.Limit);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });

        }
        [HttpPost, AllowAnonymous]
        public IActionResult QiniuFile()
        {
            var files = Request.Form.Files[0];
            var data = _uploadHelper.Upload(files, "article/");
            //TinyMCE 指定的返回格式
            return Ok(new { location = data });
        }
    }
}