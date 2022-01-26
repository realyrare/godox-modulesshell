using AutoMapper;
using GodOx.Auth.API.Common;
using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Controllers;
using GodOx.Auth.API.Models.Entity;
using GodOx.Blog.API.Enums;
using GodOx.Blog.API.Models.Dtos.Input;
using GodOx.Blog.API.Models.Dtos.Output;
using GodOx.Blog.API.Models.Entity;
using GodOx.Share.Caches;
using GodOx.Share.Repository;
using GodOx.Share.Repository.Extensions;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类名：IndexController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/2 17:11:25
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Blog.API.Controllers
{
    /// <summary>
    /// 内容管理控制器
    /// </summary>
    public class CmsController : AppBaseController
    {
        private readonly IBaseServer<AdvList> _advlistService;
        private readonly IBaseServer<Column> _columnService;
        protected ICacheHelper _cache;
        private readonly IBaseServer<Message> _messageService;
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;
        private readonly IBaseServer<Tenant> _tenantService;
        public CmsController(IBaseServer<AdvList> advlistService
            , IBaseServer<Column> columnService,
            ICacheHelper cache, IBaseServer<Tenant> tenantService, IBaseServer<Message> messageService, IMapper mapper, DbContext dbContext)
        {
            _advlistService = advlistService;
            _columnService = columnService;
            _messageService = messageService;
            _mapper = mapper;
            _dbContext = dbContext;
            _tenantService = tenantService;
            _cache = cache;
        }

        private async Task<List<Column>> GetColumnAsync(int siteId)
        {
            return await _columnService.GetListAsync(d => d.Status == true && d.TenantId == siteId);
        }
        [HttpGet]
        public async Task<ApiResult> GetAllColumn(int siteId)
        {
            var data = await GetColumnAsync(siteId);
            return new ApiResult(data);
        }
        /// <summary>
        /// 请求站点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetSiteInfo(int siteId)
        {
            var model = _tenantService.GetModelAsync(d => d.Id.Equals(siteId));
            if (model == null)
            {
                throw new ArgumentNullException("请求的站点信息为空");
            }
            return new ApiResult(model);
        }
        [HttpGet]
        public async Task<ApiResult> GetRightData(string spell, int siteId)
        {
            var monthArticle = await GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && SqlFunc.DateIsSame(DateTime.Now, ca.CreateTime, DateType.Month), 1, 10);
            var currentSpellArticle = await GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && cc.EnTitle.Trim().Equals(spell), 1, 10);
            var allChildColumn = (await GetColumnAsync(siteId)).Where(d => d.ParentId != 0).ToList();
            return new ApiResult(data: new { monthArticle = monthArticle.Items, currentSpellArticle = currentSpellArticle.Items, allChildColumn });
        }
        [HttpGet]
        public async Task<ApiResult> GetIndex(int siteId)
        {
            var recArticleList = await GetArtcileByConditionAsync((ca, cc) => ca.IsTop == true && ca.Audit == true && ca.TenantId == siteId, 1, 6);
            var categoryArticleList = await GetArtcileByConditionAsync((ca, cc) => ca.Audit == true && ca.TenantId == siteId, 1, 12);
            return new ApiResult(data: new
            {
                recArticleList = recArticleList.Items,
                categoryArticleList = categoryArticleList.Items
            });
        }
        [HttpGet]
        public async Task<ApiResult> GetAllTags(string spell, int siteId)
        {
            // 进来可能是大类或子类，1、大类下面有子类，2、大类下面没有子类 3、进来的是子类
            if (string.IsNullOrEmpty(spell))
            {
                throw new ArgumentNullException("栏目url不能为空");
            }
            var model = (await GetColumnAsync(siteId)).Where(d => d.EnTitle.Equals(spell)).FirstOrDefault();
            List<int> columnListId = new List<int>();
            if (model.ParentId == 0)
            {
                //查下该大类有没有子类
                columnListId = (await GetColumnAsync(siteId)).Where(d => d.ParentId == model.Id).Select(d => d.Id).ToList();
                //没有子类直接赋值大类id
                if (columnListId.Count == 0)
                {
                    columnListId.Add(model.Id);
                }
            }
            else
            {
                //进来了子类
                columnListId.Add(model.Id);
            }
            var list = await _dbContext.Db.Queryable<Article>().Where(d => d.Audit == true && columnListId.Contains(d.ColumnId)).GroupBy(d => d.Tag).Select(d => d.Tag).ToListAsync();
            return new ApiResult(list);
        }
        /// <summary>
        /// 根据ID查询案例/新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetDetail(int id, int siteId, string parentColumnSpell = null, string childColumnspell = null)
        {
            List<Column> columnList = await GetColumnAsync(siteId);
            if (string.IsNullOrEmpty(parentColumnSpell) && string.IsNullOrEmpty(childColumnspell))
            {
                throw new ArgumentNullException("栏目不能为空");
            }
            string columnUrl = string.IsNullOrEmpty(childColumnspell) ? parentColumnSpell : childColumnspell;
            var model = await GetArtcileDetailAsync((ca, cc) => ca.Id == id && ca.Audit == true && cc.EnTitle.Trim() == columnUrl.Trim());
            //var model=  await _cache.GetOrSetAsync($"article:{id}", 
            //async ()=> {
            //    return await _articleService.GetArtcileDetailAsync((ca, cc) => ca.Id == id && ca.Audit == true && cc.EnTitle.Trim() == columnUrl.Trim());
            //    },null);
            if (model == null)
            {
                throw new ArgumentNullException("文章详情没有查到数据");
            }
            var upArticle = await GetNextOrUpArticleAsync((ca, cc) => ca.Id > id && ca.ColumnId == model.ColumnId && ca.TenantId == siteId);
            var nextArticle = await GetNextOrUpArticleAsync((ca, cc) => ca.Id < id && ca.ColumnId == model.ColumnId && ca.TenantId == siteId);
            var sameColumnArticle = await GetArtcileByConditionAsync((ca, cc) => ca.ColumnId == model.ColumnId && ca.Audit == true && ca.TenantId == siteId, 1, 6);

            return new ApiResult(new { articleDetailModel = model, upArticle, nextArticle, sameColumnArticle = sameColumnArticle.Items });
        }
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="parentColumnSpell"></param>
        /// <param name="childColumnSpell"></param>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetList(int siteId, string parentColumnSpell, string childColumnSpell, string keyword, int page = 1)
        {
            Column columnModel = null;
            List<int> allChildColumnIdList = new List<int>();
            var columnList = await GetColumnAsync(siteId);
            //keyword
            Expression<Func<Article, Column, bool>> expression = (ca, cc) => ca.TenantId == siteId;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = HttpUtility.UrlDecode(keyword);
                expression = (ca, cc) => ca.KeyWord.Contains(keyword) || ca.Tag.Contains(keyword);
            }
            //先判断大类url是否为空
            if (!string.IsNullOrEmpty(parentColumnSpell))
            {
                var parentColumnModel = columnList.Where(d => d.ParentId == 0 && d.EnTitle.Equals(parentColumnSpell.Trim())).FirstOrDefault();
                if (parentColumnModel != null)
                {
                    columnModel = parentColumnModel;
                    allChildColumnIdList = columnList.Where(d => d.ParentId == parentColumnModel.Id && d.ParentId != 0).Select(d => d.Id).ToList();
                    //如果这个大栏目没有子栏目时就根据大栏目去查找对应的文章
                    if (allChildColumnIdList.Count == 0)
                    {
                        expression = (ca, cc) => ca.ColumnId == columnModel.Id;
                    }
                }
            }
            //判断allChildColumnIdList是否有值
            if (allChildColumnIdList.Count > 0 && allChildColumnIdList.Any())
            {
                expression = (ca, cc) => allChildColumnIdList.Contains(ca.ColumnId);
            }
            //判断子类url是否为空
            if (!string.IsNullOrEmpty(childColumnSpell))
            {
                var childColumnModel = columnList.Where(d => d.ParentId > 0 && d.EnTitle.Equals(childColumnSpell.Trim())).FirstOrDefault();
                if (childColumnModel != null)
                {
                    columnModel = childColumnModel;
                    expression = (ca, cc) => ca.ColumnId == childColumnModel.Id;
                }
            }

            var query = await GetArtcileByConditionAsync(expression, page, 15);

            return new ApiResult(new
            {
                ArticleList = query.Items,
                total = query.TotalItems,
                columnModel?.Title,
                Keywords = columnModel?.Keyword,
                Description = columnModel?.Summary
            });
        }
        /// <summary>
        ///  用户留言，提交需求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> AddMsg([FromBody] MessageInput messageInput)
        {
            messageInput.IP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress.ToString();

            var list = await _messageService.GetListAsync(m => m.IP == messageInput.IP && m.TenantId == messageInput.TenantId && m.Types == messageInput.Types, m => m.CreateTime, true);
            if (list.Count() > 3)
            {
                throw new ArgumentNullException("您提交的次数过多，请稍后重试！~");
            }
            messageInput.Address = IpParseHelper.GetAddressByIP(messageInput.IP);
            var model = _mapper.Map<Message>(messageInput);
            await _messageService.AddAsync(model);
            return new ApiResult();
        }
        [HttpGet]
        public async Task<ApiResult> LoadMessage(int businessId, int siteId, string types)
        {
            var result = await _messageService.GetListAsync(x => x.BusinessId == businessId && x.TenantId.Equals(siteId) && x.Types.Equals(types), x => x.CreateTime, false);
            return new ApiResult(result);
        }
        [HttpGet]
        public async Task<ApiResult> GetAdvList(int siteId, AdvEnum type)
        {
            var advList = await _advlistService.GetListAsync(x => x.Status == true && x.TenantId == siteId && x.Type == type, x => x.Sort, false);
            return new ApiResult(advList);
        }


        private async Task<Page<ArticleOutput>> GetArtcileByConditionAsync(Expression<Func<Article, Column, bool>> where, int pageIndex, int pageSize)
        {
            return await _dbContext.Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                  .WhereIF(where != null, where)
                  .OrderBy((ca, cc) => ca.Id, OrderByType.Desc)
                  .Select((ca, cc) => new ArticleOutput
                  {
                      Title = ca.Title,
                      Id = ca.Id,
                      ColumnId = ca.ColumnId,
                      Summary = ca.Summary,
                      EnTitle = cc.EnTitle,
                      Author = ca.Author,
                      Source = ca.Source,
                      Tag = ca.Tag,
                      CreateTime = ca.CreateTime,
                      ColumnName = cc.Title,
                      Content = ca.Content,
                      ThumImg = ca.ThumImg,
                      IsTop = ca.IsTop,
                      IsHot = ca.IsHot,
                      ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                  })
                  .ToPageAsync(pageIndex, pageSize);
        }
        private async Task<ArticleOutput> GetArtcileDetailAsync(Expression<Func<Article, Column, bool>> where)
        {
            return await _dbContext.Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                   .WhereIF(where != null, where)
                  .Select((ca, cc) => new ArticleOutput
                  {
                      Title = ca.Title,
                      Id = ca.Id,
                      ColumnId = ca.ColumnId,
                      Summary = ca.Summary,
                      EnTitle = cc.EnTitle,
                      Author = ca.Author,
                      Source = ca.Source,
                      Tag = ca.Tag,
                      CreateTime = ca.CreateTime,
                      Content = ca.Content,
                      ThumImg = ca.ThumImg,
                      ColumnName = cc.Title,
                      KeyWord = ca.KeyWord,
                      ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                  })
                  .FirstAsync();
        }
        private async Task<ArticleOutput> GetNextOrUpArticleAsync(Expression<Func<Article, Column, bool>> expression)
        {
            //(ca, cc) => ca.Id < id && ca.ColumnId == columnId
            return await _dbContext.Db.Queryable<Article, Column>((ca, cc) => new object[] { JoinType.Inner, ca.ColumnId == cc.Id })
                .Where(expression)
                .OrderBy((ca, cc) => ca.Id, OrderByType.Desc)
                .Select((ca, cc) => new ArticleOutput
                {
                    Title = ca.Title,
                    Id = ca.Id,
                    ColumnId = ca.ColumnId,
                    EnTitle = cc.EnTitle,
                    ParentColumnUrl = SqlFunc.Subqueryable<Column>().Where(s => s.Id == cc.ParentId).Select(s => s.EnTitle)
                })
                .FirstAsync();
        }
    }
}