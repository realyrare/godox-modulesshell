using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{
    public class LogsController : ApiControllerBase
    {
        private readonly IBaseServer<Log> _logService;
        public LogsController(IBaseServer<Log> logService)
        {
            _logService = logService;
        }


        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _logService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages(int page, int limit = 15, string key = null)
        {
            Expression<Func<Log, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Message.Contains(key);
            }
            var res = await _logService.GetPagesAsync(page, limit, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet, Authority]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _logService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
    }
}
