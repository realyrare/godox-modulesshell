using AutoMapper;
using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Entity.Common;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System.Threading.Tasks;

/*************************************
* 类名：ApiBaseController
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/1 10:28:20
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Sys.API.Controllers
{
    /// <summary>
    /// 适用于非多租户的模块使用
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDetailQuery"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TListQuery"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    [ApiController]
    public abstract class ApiBaseController<TEntity, TDetailQuery, TDeleteInput, TListQuery, TCreateInput, TUpdateInput> : ControllerBase
       where TEntity : BaseEntity, new()
       where TDeleteInput : DeletesInput
       where TDetailQuery : DetailQuery
       where TListQuery : PageQuery
    {
        private readonly IBaseServer<TEntity> _service;
        private readonly IMapper _mapper;

        public ApiBaseController(IBaseServer<TEntity> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpDelete, Authority]
        public virtual async Task<ApiResult> Deletes([FromBody] TDeleteInput commonDeleteInput)
        {
            var res = await _service.DeleteAsync(commonDeleteInput.Ids);
            return res <= 0 ? new ApiResult("删除失败了！") : new ApiResult();
        }
        [HttpGet, Authority]
        public virtual async Task<ApiResult> GetListPages([FromQuery] TListQuery listQuery)
        {
            var res = await _service.GetPagesAsync(listQuery.Page, listQuery.Limit, d => d.Status == true, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        [HttpGet, Authority]
        public virtual async Task<ApiResult> Detail([FromQuery] TDetailQuery detailQuery)
        {
            var res = await _service.GetModelAsync(d => d.Id == detailQuery.Id && d.Status == true);
            return new ApiResult(data: res);
        }
        [HttpPost]
        public virtual async Task<ApiResult> Add([FromBody] TCreateInput createInput)
        {
            var entity = _mapper.Map<TEntity>(createInput);
            var res = await _service.AddAsync(entity);
            return res <= 0 ? new ApiResult("添加失败了！") : new ApiResult();
        }
        [HttpPut, Authority]
        public virtual async Task<ApiResult> Modify([FromBody] TUpdateInput updateInput)
        {
            var entity = _mapper.Map<TEntity>(updateInput);
            var res = await _service.UpdateAsync(entity);
            return res <= 0 ? new ApiResult("修改失败了！") : new ApiResult();
        }
    }
}