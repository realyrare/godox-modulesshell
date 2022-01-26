using GodOx.Share.FileManage;
using GodOx.Shop.API.Models.Dtos.Input;
using GodOx.Shop.API.Services;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Controllers;
using GodOx.Sys.API.Models.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GodOx.Sys.API.Attributes;
using System.Threading.Tasks;

namespace GodOx.Shop.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    [Route("api/shop/[controller]/[action]")]
    [MultiTenant]
    public class GoodsController : ApiControllerBase
    {
        private readonly IGoodsService _goodsService;
        private readonly IUploadFile _uploadHelper;

        public GoodsController(IGoodsService goodsService, IUploadFile uploadHelper)
        {
            _goodsService = goodsService;
            _uploadHelper = uploadHelper;
        }

        [HttpGet, Authority]
        public Task<ApiResult> GetListPages([FromQuery] KeyListTenantQuery query)
        {
            return _goodsService.GetListPageAsync(query);
        }
        [HttpPost, Authority]
        public async Task<ApiResult> Add([FromBody] GoodsInput input)
        {
            return await _goodsService.AddAsync(input);
        }
        [HttpPut, Authority]
        public async Task<ApiResult> Modify([FromBody] GoodsModifyInput input)
        {
            return await _goodsService.ModifyAsync(input);
        }
        [HttpPost]
        public Task<ApiResult> AddSpec([FromForm] SpecInput input)
        {
            return _goodsService.AddSpecAsync(input);
        }
        [HttpPost]
        public Task<ApiResult> AddSpecValue([FromForm] SpecValuesInput input)
        {
            return _goodsService.AddSpecAsync(input);
        }
        [HttpPost, AllowAnonymous]
        public IActionResult UploadImg()
        {
            var files = Request.Form.Files[0];
            var result = _uploadHelper.Upload(files, "goods/");
            //TinyMCE 指定的返回格式
            return Ok(new { location = result });
        }
        [HttpPost]
        public ApiResult MultipleUploadImg([FromForm] IFormCollection formData)
        {
            var data = _uploadHelper.Upload(formData.Files, "goods/");
            //TinyMCE 指定的返回格式
            return new ApiResult(data);
        }
    }
}
