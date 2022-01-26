using GodOx.Auth.API.Configs;
using GodOx.Auth.API.Models.Dtos.Output;
using Microsoft.AspNetCore.Mvc;
using GodOx.Auth.API.Attributes;

/*************************************
* 类名：MiniAppBaseController
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/27 15:34:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Auth.API.Controllers
{

    [Route("api/app/[controller]/[action]")]
    [ApiController]
    [AppAuth]
    public class AppBaseController : ControllerBase
    {
        public HttpWxUserOutput HttpWx => CurrentAppContext.WxUser;
        [NonAction]
        public ApiResult Result(int i) => i > 0 ? new ApiResult() : new ApiResult("操作失败了！");


    }
}