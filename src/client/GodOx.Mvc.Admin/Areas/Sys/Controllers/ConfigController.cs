using GodOx.Share.Repository;
using GodOx.Sys.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GodOx.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class ConfigController : Controller
    {
        private readonly IBaseServer<Config> _configService;

        public ConfigController(IBaseServer<Config> configService)
        {
            _configService = configService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            var model = id == 0 ? new Config() : await _configService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
