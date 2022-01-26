using Microsoft.AspNetCore.Mvc;

namespace GodOx.Sys.API.Controllers
{
    [Route("api/sys/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {

    }
}
