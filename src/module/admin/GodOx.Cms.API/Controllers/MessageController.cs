using GodOx.Cms.API.Models.Entity;
using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using GodOx.Sys.API.Models.Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

/*************************************
* 类名：MessageController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/16 14:40:43
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Controllers
{
    [Route("api/cms/[controller]/[action]")]
    [MultiTenant]
    public class MessageController : ApiControllerBase
    {
        private readonly IBaseServer<Message> _service;
        public MessageController(IBaseServer<Message> service)
        {
            _service = service;
        }
        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery keywordListTenantQuery)
        {
            Expression<Func<Message, bool>> whereExpression = d => d.Status == true;
            if (keywordListTenantQuery.TenantId > 0)
            {
                whereExpression = d => d.TenantId == keywordListTenantQuery.TenantId;
            }
            if (!string.IsNullOrEmpty(keywordListTenantQuery.Key))
            {
                whereExpression = d => d.UserName.Contains(keywordListTenantQuery.Key);
            }
            var res = await _service.GetPagesAsync(keywordListTenantQuery.Page, keywordListTenantQuery.Limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 批量真实删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesTenantInput deleteInput)
        {
            var res = await _service.DeleteAsync(deleteInput.Ids);
            if (res <= 0)
            {
                return new ApiResult("删除失败了！");
            }
            return new ApiResult();
        }
    }
}