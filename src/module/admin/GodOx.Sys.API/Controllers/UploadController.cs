using GodOx.Share.FileManage;
using GodOx.Sys.API.Configs;
using GodOx.Sys.API.Models.Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GodOx.Sys.API.Controllers
{
    /// <summary>
    /// 商品分类控制器
    /// </summary>
    public class UploadController : ApiControllerBase
    {
        private readonly IUploadFile _uploadHelper;
        public UploadController(IUploadFile uploadHelper)
        {
            _uploadHelper = uploadHelper;
        }
        [HttpPost]
        public ApiResult File([FromBody] UploadInput input)
        {
            if (string.IsNullOrEmpty(input.Directory))
            {
                throw new ArgumentNullException("图片的上传目录不能为空！");
            }
            var files = Request.Form.Files;
            var data = _uploadHelper.Upload(files, input.Directory);
            return new ApiResult(data);
        }
    }
}
