using AutoMapper;
using GodOx.Share.Repository;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using GodOx.Sys.API.Models.Dtos.Input;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GodOx.Sys.API.Controllers
{
    public class ConfigController : ApiControllerBase
    {
        private readonly IBaseServer<Config> _configService;
        private readonly IMapper _mapper;

        public ConfigController(IBaseServer<Config> configService, IMapper mapper)
        {
            _configService = configService;
            _mapper = mapper;
        }
        [HttpDelete, Authority]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _configService.DeleteAsync(commonDeleteInput.Ids));
        }

        [HttpGet, Authority]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Config, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Name.Contains(key);
            }
            var res = await _configService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        [HttpGet, Authority]
        public async Task<ApiResult> Detail(int id)
        {
            var res = await _configService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }

        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] ConfigInput input)
        {
            var model = await _configService.GetModelAsync(d => d.EnName.Equals(input.EnName));
            if (model.Id > 0)
            {
                throw new ArgumentNullException("英文名称已存在");
            }
            var modelInput = _mapper.Map<Config>(input);
            var res = await _configService.AddAsync(modelInput);
            return new ApiResult(data: res);
        }
        [HttpPut, Authority]
        public async Task<ApiResult> Modify([FromBody] ConfigModifyInput input)
        {
            var model = await _configService.GetModelAsync(d => d.EnName.Equals(input.EnName) && d.Id != input.Id);
            if (model.Id > 0)
            {
                throw new ArgumentNullException("英文名称已存在");
            }
            var res = await _configService.UpdateAsync(d => new Config()
            {
                Name = input.Name,
                EnName = input.EnName,
                Type = input.Type,
                ModifyTime = DateTime.Now,
                Summary = input.Summary
            }, d => d.Id == input.Id);
            return new ApiResult(data: res);
        }
    }
}
